using ApiTP6.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTP6.DTOs;
using ApiTP6.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiTP6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CarritoController(AppDbContext context)
        {
            _context = context;
        }

        #region Listado
        [HttpGet]
        [Authorize(Roles = "Admin,Empleado")]
        [Route("lista")]
        public async Task<ActionResult<List<CarritoDTO>>> Get()
        {
            var carritosDB = await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.DetallesCarritos)
                .ThenInclude(dc => dc.Producto)
                .ToListAsync();

            var carritosDTO = carritosDB.Select(carrito => new CarritoDTO
            {
                IDCarrito = carrito.IDCarrito,
                Fecha = carrito.Fecha,
                IDUsuario = carrito.Usuario.IDUsuario,
                Productos = carrito.DetallesCarritos.Select(c => new DetallesCarritoDTO
                {
                    IDDetallesCarrito = c.IDDetallesCarrito,
                    IDProducto = c.IDProducto,
                    Cantidad = c.Cantidad
                }).ToList()
            }).ToList();

            return Ok(carritosDTO);
        }
        #endregion


        #region Buscar
        [HttpGet]
        [Authorize(Roles = "Admin,Empleado,Usuario")]
        [Route("buscar/{id}")]
        public async Task<ActionResult<CarritoDTO>> Get(int id)
        {
            var carritoDTO = new CarritoDTO();
            var carritoDB = await _context.Carritos.Include(c => c.Usuario)
                .Include(c => c.DetallesCarritos)
                .ThenInclude(dc => dc.Producto)
                .Where(c => c.IDCarrito == id)
                .FirstOrDefaultAsync();

            if (carritoDB == null)
            {
                return NotFound();
            }

            carritoDTO.IDCarrito = id;
            carritoDTO.Fecha = carritoDB.Fecha;
            carritoDTO.IDUsuario = carritoDB.Usuario.IDUsuario;
            carritoDTO.Productos = carritoDB.DetallesCarritos.Select(c => new DetallesCarritoDTO
            {
                IDDetallesCarrito = c.IDDetallesCarrito,
                IDProducto = c.IDProducto,
                Cantidad = c.Cantidad,
            }).ToList();


            return Ok(carritoDTO);
        }
        #endregion

        #region Crear
        [HttpPost]
        [Authorize(Roles = "Admin,Usuario,Empleado")]
        [Route("crear")]
        public async Task<ActionResult<CarritoDTO>> Crear(CarritoDTO carritoDTO)
        {
            // para crear un carrito necesito guardar la fecha, el idusuario y crear las instancias de detalles carrito que toma idproducto y cantidad
            if (carritoDTO == null || carritoDTO.Productos == null || !carritoDTO.Productos.Any())
            {
                return BadRequest("Carrito o productos inválidos.");
            }
            var carritoDB = new Carrito
            {
                Fecha = carritoDTO.Fecha,
                IDUsuario = carritoDTO.IDUsuario,
                DetallesCarritos = new List<DetallesCarrito>()
            };

            //añade los productos al carrito
            foreach(var productoDTO in carritoDTO.Productos)
            {
                var producto = await _context.Productos.FindAsync(productoDTO.IDProducto);  //busca si existe

                if (producto == null)
                {
                    return BadRequest("El producto con ID {productoDTO.ProductId} no existe.");
                }

                //crear detalle del carrito
                var detallesCarrito = new DetallesCarrito
                {
                    IDProducto = productoDTO.IDProducto,
                    Cantidad = productoDTO.Cantidad,
                    Producto = producto
                };
                carritoDB.DetallesCarritos.Add(detallesCarrito);
            }
            
            await _context.Carritos.AddAsync(carritoDB);
            await _context.SaveChangesAsync();
            return Ok(carritoDTO.IDCarrito);
        }
        #endregion

        #region Editar
        // EDITAR CARRITO IMPLICA EDITAR CARRITO Y LOS DETALLES CARRITOS
        [HttpPut]
        [Authorize(Roles = "Admin,Usuario,Empleado")]
        [Route("editar/{id}")]
        public async Task<ActionResult<CarritoDTO>> Editar(int id, [FromBody] CarritoDTO carritoDTO)
        {
            var carritoDB = await _context.Carritos.Include(c => c.Usuario)
                .Include(c => c.DetallesCarritos)
                .ThenInclude(dc => dc.Producto)
                .Where(c => c.IDCarrito == id)
                .FirstOrDefaultAsync();

            if (carritoDB == null)
            {
                return NotFound("Carrito no encontrado.");
            }

            carritoDB.Fecha = carritoDTO.Fecha;
            carritoDB.IDUsuario = carritoDTO.IDUsuario;

            // Actualizar los detalles del carrito
            foreach (var detalleDTO in carritoDTO.Productos)
            {
                var detalleDB = carritoDB.DetallesCarritos
                    .FirstOrDefault(d => d.IDDetallesCarrito == detalleDTO.IDDetallesCarrito);

                if (detalleDB != null)
                {
                    // Actualizar detalle existente
                    detalleDB.Cantidad = detalleDTO.Cantidad;
                    detalleDB.IDProducto = detalleDTO.IDProducto;
                } 
                else
                {
                    // Agregar nuevo detalle si no existe
                    carritoDB.DetallesCarritos.Add(new DetallesCarrito
                    {
                        IDProducto = detalleDTO.IDProducto,
                        Cantidad = detalleDTO.Cantidad,
                        IDCarrito = carritoDTO.IDCarrito
                    });
                }
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Carrito actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar los datos: {ex.Message}");
            }
        }
        #endregion

        #region Eliminar
        //ELIMINAR CARRITO IMPLICA ELIMINAR CARRITO Y DETALLES CARRITO
        [HttpDelete]
        [Authorize(Roles = "Admin,Empleado")]
        [Route("eliminar/{id}")]
        public async Task<ActionResult<CarritoDTO>> Eliminar(int id)
        {
            var carritoDB = await _context.Carritos.FindAsync(id);

            if (carritoDB is null)
            {
                return NotFound("Carrito no encontrado");
            }

            _context.Carritos.Remove(carritoDB);

            await _context.SaveChangesAsync();

            return Ok("Carrito eliminado");
        }
        #endregion

    }
}
