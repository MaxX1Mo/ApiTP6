using Microsoft.AspNetCore.Identity;

namespace ApiTP6.Models
{
    public class Usuario
    {
        public int IDUsuario { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int IDPersona { get; set; }

        public virtual Persona Persona { get; set; }

        
        public virtual ICollection<Carrito> Carrito { get; set; }
    }

}
