using ApiTP6.Models;

namespace ApiTP6.DTOs
{
    public class CarritoDTO
    {
        public int IDCarrito { get; set; }
        public DateTime Fecha { get; set; }
        public int IDUsuario { get; set; }
        public List<DetallesCarritoDTO> Productos { get; set; }

    }
}
