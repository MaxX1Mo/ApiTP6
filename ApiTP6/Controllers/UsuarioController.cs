using ApiTP6.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTP6.DTOs;
using ApiTP6.Models;
using ApiTP6.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace ApiTP6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Seguridad _seguridad;

        public UsuarioController(AppDbContext context, Seguridad seguridad)
        {
            _context = context;
            _seguridad = seguridad;
        }

        #region Listado
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
                    Rol = item.Rol,
                    //Password = item.Password,
                    IDPersona = item.Persona.IDPersona,
                    Nombre= item.Persona.Nombre,
                    Apellido = item.Persona.Apellido,
                    NroCelular = item.Persona.NroCelular,
                });
            }
            return Ok(listaDTO);
        }
        #endregion

        #region Buscar
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
            usuarioDTO.Rol = usuarioDB.Rol;
            //usuarioDTO.Password = usuarioDB.Password;
            usuarioDTO.IDPersona = usuarioDB.Persona.IDPersona;
            usuarioDTO.Nombre = usuarioDB.Persona.Nombre;
            usuarioDTO.Apellido = usuarioDB.Persona.Apellido;
            usuarioDTO.NroCelular = usuarioDB.Persona.NroCelular;

            return Ok(usuarioDTO);
        }
        #endregion

        #region CREAR
        [HttpPost]
        [AllowAnonymous]
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
                Password = _seguridad.encriptarSHA256(usuarioDTO.Password),
                Rol = usuarioDTO.Rol,
                Persona = personaDB
            };

            await _context.Usuarios.AddAsync(usuarioDB);
            await _context.SaveChangesAsync();
            return Ok("Usuario Creado");
        }

        #endregion
        #region CREAR para registro usuario
        [HttpPost]
        [AllowAnonymous]
        [Route("registro")]
        public async Task<ActionResult<UsuarioDTO>> Registro(UsuarioDTO usuarioDTO)
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
                Password = _seguridad.encriptarSHA256(usuarioDTO.Password),
                Rol = Roles.Usuario,
                Persona = personaDB
            };

            await _context.Usuarios.AddAsync(usuarioDB);
            await _context.SaveChangesAsync();
            return Ok("Usuario Creado");
        }
        #endregion

        #region EDITAR
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("editar/")]
        public async Task<ActionResult<UsuarioDTO>> Editar(int id,UsuarioDTO usuarioDTO)
        {
            var usuariodb = await _context.Usuarios
                            .Include(u => u.Persona)
                            .FirstOrDefaultAsync(u => u.IDUsuario == id);

            if (usuariodb == null)
            {
                return NotFound("Usuario no encontrado.");
            }


            usuariodb.Email = usuarioDTO.Email;
            usuariodb.Username = usuarioDTO.Username;
            usuariodb.Password = _seguridad.encriptarSHA256(usuarioDTO.Password);
            usuariodb.Rol = usuarioDTO.Rol;

            if (usuariodb.Persona != null)
            {
                usuariodb.Persona.Nombre = usuarioDTO.Nombre;
                usuariodb.Persona.Apellido = usuarioDTO.Apellido;
                usuariodb.Persona.NroCelular = usuarioDTO.NroCelular;
            }


            try
            {
                await _context.SaveChangesAsync();
                return Ok("Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar los datos: {ex.Message}");
            }
        }
        #endregion

        #region Eliminar
        [HttpDelete]
        [Authorize(Roles = "Admin")]
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
        #endregion
    }
}
