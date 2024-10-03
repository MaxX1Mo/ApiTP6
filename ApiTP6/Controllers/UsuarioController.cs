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
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<UsuarioDTO>>> Get()
        {
            var listaDTO = new List<UsuarioDTO>();
            var listaDB = await _context.Usuarios.Include(c => c.Persona).ToListAsync();


            foreach (var item in listaDB)
            {
                listaDTO.Add(new UsuarioDTO
                {
                    IDUsuario = item.IDUsuario,
                    Email = item.Email,
                    Username = item.Username,
                    //Password = item.Password,
                    IDPersona = item.IDPersona,
                    Nombre= item.Persona.Nombre,
                    Apellido = item.Persona.Apellido,
                    NroCelular = item.Persona.NroCelular,
                });
            }
            return Ok(listaDTO);
        }

        [HttpGet]
        [Route("buscar/{id}")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            var usuarioDTO = new UsuarioDTO();
            var usuarioDB = await _context.Usuarios.Include(u => u.Persona).Where(c => c.IDUsuario == id).FirstOrDefaultAsync();

            if (usuarioDB == null)
            {
                return NotFound();
            }

            usuarioDTO.IDUsuario = id;
            usuarioDTO.Email = usuarioDB.Email;
            usuarioDTO.Username = usuarioDB.Username;
            //usuarioDTO.Password = usuarioDB.Password;
            usuarioDTO.IDPersona = usuarioDB.IDPersona;
            usuarioDTO.Nombre = usuarioDB.Persona.Nombre;
            usuarioDTO.Apellido = usuarioDB.Persona.Apellido;
            usuarioDTO.NroCelular = usuarioDB.Persona.NroCelular;

            return Ok(usuarioDTO);
        }

        [HttpPost]
        [Route("crear")]
        public async Task<ActionResult<UsuarioDTO>> Crear(UsuarioDTO usuarioDTO)
        {

            var personaDB = new Persona
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                NroCelular = usuarioDTO.NroCelular
            };
            var usuarioDB = new Usuario
            {
                Email = usuarioDTO.Email,
                Username = usuarioDTO.Username,
                Password = usuarioDTO.Password,
                Persona = personaDB,
            };

            await _context.Usuarios.AddAsync(usuarioDB);
            await _context.SaveChangesAsync();
            return Ok("Usuario Creado");
        }

         Editar USUARIO IMPLICA Editar USUARIO Y PERSONA
          
        [HttpPut]
        [Route("editar")]
        public async Task<ActionResult<CarritoDTO>> Editar(CarritoDTO carritoDTO)
        {
            var carritoDB = await _context.Productos
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

        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<ActionResult<UsuarioDTO>> Eliminar(int id)
        {
            var usuarioDB = await _context.Usuarios.FindAsync(id);

            if (usuarioDB is null)
            {
                return NotFound("Usuario no encontrado");
            }

            _context.Usuarios.Remove(usuarioDB);

            await _context.SaveChangesAsync();

            return Ok("Usuario eliminado");
        }
        
    }
}
