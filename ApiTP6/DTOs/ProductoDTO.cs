namespace ApiTP6.DTOs
{
    public class ProductoDTO
    {
        public int IDProducto { get; set; }
        public string? NombreProducto { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string? Imagen { get; set; }
        public int Stock { get; set; }
    }
}
