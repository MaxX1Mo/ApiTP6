﻿using ApiTP6.Data;
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

        #region Listado
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
        #endregion

        #region Buscar
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
        #endregion

        #region CREAR
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
        #endregion

        #region EDITAR
        [HttpPut]
        [Route("editar/{id}")]
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
            usuariodb.Password = usuarioDTO.Password;  // realizar un proceso de hashing a futuro para el password


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
