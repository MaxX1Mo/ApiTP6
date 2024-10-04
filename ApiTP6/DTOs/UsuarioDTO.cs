namespace ApiTP6.DTOs
{
    public class UsuarioDTO
    {
        public int IDUsuario { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int IDPersona { get; set; }
        public string? Nombre { get; set;}
        public string? Apellido { get; set; }
        public string? NroCelular { get; set; }
        public Roles Rol { get; set; }

    }
}
