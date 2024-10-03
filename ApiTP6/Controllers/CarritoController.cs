using ApiTP6.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTP6.DTOs;
using ApiTP6.Models;

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

        [HttpGet]
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
                    IDProducto = c.IDProducto,
                    Cantidad = c.Cantidad
                }).ToList()
            }).ToList();

            return Ok(carritosDTO);
        }


        [HttpGet]
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
                IDProducto = c.IDProducto,
                Cantidad = c.Cantidad,
            }).ToList();


            return Ok(carritoDTO);
        }


        [HttpPost]
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
            return Ok("Carrito Creado");
        }



        // EDITAR CARRITO IMPLICA EDITAR CARRITO Y LOS DETALLES CARRITOS
        /*
        [HttpPut]
        [Route("editar")]
        public async Task<ActionResult<CarritoDTO>> Editar(CarritoDTO carritoDTO)
        {
            var productoDB = await _context.Productos
                .Where(p => p.IDProducto == productoDTO.IDProducto).FirstOrDefaultAsync();

            productoDB.NombreProducto = productoDTO.NombreProducto;
            productoDB.Descripcion = productoDTO.Descripcion;
            productoDB.Precio = productoDTO.Precio;
            productoDB.Imagen = productoDTO.Imagen;
            productoDB.Stock = productoDTO.Stock;

            _context.Productos.Update(productoDB);
            await _context.SaveChangesAsync();
            return Ok("Producto modificado");
        }

        //ELIMINAR CARRITO IMPLICA ELIMINAR CARRITO Y DETALLES CARRITO
        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<ActionResult<CarritoDTO>> Eliminar(int id)
        {
            var carritoDB = await _context.Carritos.FindAsync(id);

            if (carritoDB is null)
            {
                return NotFound("Producto no encontrado");
            }

            _context.Carritos.Remove(carritoDB);

            await _context.SaveChangesAsync();

            return Ok("Producto eliminado");
        }
        /*

    }
}
