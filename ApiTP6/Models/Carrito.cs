namespace ApiTP6.Models
{
    public class Carrito
    {
        public int IDCarrito { get; set; }
        public DateTime Fecha { get; set; }
        public int IDUsuario { get; set; }
        
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<DetallesCarrito> DetallesCarritos { get; set; }
    }
}
