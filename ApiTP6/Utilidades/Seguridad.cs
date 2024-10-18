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

namespace ApiTP6.Utilidades
{
    public class Seguridad
    {
        private readonly IConfiguration _configuration;
        
        public Seguridad(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computar el hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                // Convertir el array de bytes a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public string generarJWT(Usuario usuario)
        {
            //crear la informacion del usuario para token 
            var userClaims = new[]                                   // un claim es un dato sobre el usuario que se incluye en el token
            {
            new Claim(ClaimTypes.NameIdentifier, usuario.IDUsuario.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email!),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString()) //Cuando genera el token, se agrega el rol del usuario como un "claim". Esto permite verificar el rol al autenticar al usuario.
            };
        
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));   // clave de seguridad simétrica a partir de una clave secreta almacenada en la configuración
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); //  credenciales de firma usando el algoritmo HMAC con SHA256

            //Se configura el token JWT con los claims del usuario, una expiración de 10 minutos y las credenciales de firma.
            var jwtConfig = new JwtSecurityToken(        
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig); //se escribe y devuelve el token
        }
    }
}
