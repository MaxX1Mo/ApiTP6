namespace ApiTP6.Models
{
    public class DetallesCarrito
    {
        public int IDDetallesCarrito { get; set; }
        public int Cantidad { get; set; }
        public int IDProducto { get; set; } 
        public int IDCarrito { get; set; } 

        public virtual Producto Producto { get; set; }
        public virtual Carrito Carrito { get; set; }
       
    }


}
