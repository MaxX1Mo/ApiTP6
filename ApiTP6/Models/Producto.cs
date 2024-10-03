namespace ApiTP6.Models
{
    public class Producto
    {
        public int IDProducto { get; set; }
        public string NombreProducto { get; set;}
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public int Stock { get; set; }


        public virtual ICollection<DetallesCarrito> DetallesCarritos { get; set; }

    }



}
