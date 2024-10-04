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
            var userClaims = new[]
            {
                //new Claim(ClaimTypes.NameIdentifier, modelo.IdUsuario.ToString()),
                //new Claim(ClaimTypes.Email, modelo.Correo!)
            new Claim(ClaimTypes.NameIdentifier, usuario.IDUsuario.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email!),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString()) //Cuando genera el token, se agrega el rol del usuario como un "claim". Esto permite verificar el rol al autenticar al usuario.
            };
        


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }


        /*
        private bool VerificarPassword(string passwordIngresada, string passwordAlmacenada)
        {
            // Aquí iría tu lógica para comparar el password almacenado con el que ingresó el usuario
            // Si se usa un hash para la contraseña, se realiza la verificacion aqui
            return passwordIngresada == passwordAlmacenada;
        }

        private string GenerarTokenJWT(Usuario usuario)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString()) //Cuando genera el token, se agrega el rol del usuario como un "claim". Esto permite verificar el rol al autenticar al usuario.
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }*/
    }
}
