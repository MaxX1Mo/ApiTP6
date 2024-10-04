using ApiTP6.Data;
using ApiTP6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiTP6.DTOs;
using ApiTP6.Utilidades;

namespace ApiTP6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Seguridad _seguridad;

        public LoginController( AppDbContext context, Seguridad seguridad)
        {
            _seguridad = seguridad;
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios
                .Where(u =>u.Email == loginDTO.Email && u.Password == _seguridad.encriptarSHA256(loginDTO.Password))
                .FirstOrDefaultAsync();

            if (usuario == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _seguridad.generarJWT(usuario) });
        }
    }
}
