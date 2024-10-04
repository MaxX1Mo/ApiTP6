namespace ApiTP6.Models
{
    public class Persona
    {
        public int IDPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NroCelular { get; set; }
        public int IDUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
    }


}
