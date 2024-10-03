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
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        #region Listado
        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<ProductoDTO>>> Get()
        {
            var listaDTO = new List<ProductoDTO>();
            var listaDB = await _context.Productos.ToListAsync();


            foreach (var item in listaDB)
            {
                listaDTO.Add(new ProductoDTO
                {
                    IDProducto = item.IDProducto,
                    NombreProducto = item.NombreProducto,
                    Descripcion = item.Descripcion,
                    Precio = item.Precio,
                    Imagen = item.Imagen,
                    Stock = item.Stock,

                });
            }
            return Ok(listaDTO);
        }
        #endregion

        #region Buscar
        [HttpGet]
        [Route("buscar/{id}")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)
        {
            var productoDTO = new ProductoDTO(); 
            var productoDB = await _context.Productos.Where(p => p.IDProducto == id).FirstOrDefaultAsync();
            if (productoDB == null)
            {
                return NotFound();
            }
            productoDTO.IDProducto = id;
            productoDTO.NombreProducto = productoDB.NombreProducto;
            productoDTO.Descripcion = productoDB.Descripcion;
            productoDTO.Precio = productoDB.Precio; 
            productoDTO.Imagen = productoDB.Imagen;
            productoDTO.Stock = productoDB.Stock;

            return Ok(productoDTO);
        }
        #endregion

        #region Crear
        [HttpPost]
        [Route("crear")]
        public async Task<ActionResult<ProductoDTO>> Crear(ProductoDTO productoDTO)
        {
            var productoDB = new Producto
            {
                NombreProducto = productoDTO.NombreProducto,
                Descripcion = productoDTO.Descripcion,
                Precio = productoDTO.Precio,
                Imagen = productoDTO.Imagen, 
                Stock = productoDTO.Stock
            };
            await _context.Productos.AddAsync(productoDB);
            await _context.SaveChangesAsync();
            return Ok("Producto Creado");
        }
        #endregion

        #region Editar
        [HttpPut]
        [Route("editar/{id}")]
        public async Task<ActionResult<ProductoDTO>> Editar(int id, ProductoDTO productoDTO)
        {
            var productoDB = await _context.Productos
                .Where(p => p.IDProducto == id).FirstOrDefaultAsync();

            if (productoDB == null)
            {
                return NotFound("Producto no encontrado.");
            }


            productoDB.NombreProducto = productoDTO.NombreProducto;
            productoDB.Descripcion = productoDTO.Descripcion;
            productoDB.Precio = productoDTO.Precio;
            productoDB.Imagen = productoDTO.Imagen;
            productoDB.Stock = productoDTO.Stock;

            _context.Productos.Update(productoDB);
            await _context.SaveChangesAsync();
            return Ok("Producto modificado");
        }
        #endregion

        #region Eliminar
        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<ActionResult<ProductoDTO>> Eliminar(int id)
        {
            var productoDB = await _context.Productos.FindAsync(id);
            
            if (productoDB is null)
            {
                return NotFound("Producto no encontrado");
            }
            
            _context.Productos.Remove(productoDB);

            await _context.SaveChangesAsync();  
    
            return Ok("Producto eliminado");
        }
        #endregion

    }
}
