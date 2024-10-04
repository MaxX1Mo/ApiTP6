using ApiTP6.Models;
using Microsoft.EntityFrameworkCore;
namespace ApiTP6.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<DetallesCarrito> DetallesCarritos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configuraciones de las tablas
            //PRODUCTO
            modelBuilder.Entity<Producto>(tb =>
            {
                tb.HasKey(c => c.IDProducto);
                tb.Property(c => c.IDProducto)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(p => p.NombreProducto)
                .HasMaxLength(100)
                .IsRequired();

                 tb.Property(p => p.Descripcion)
                .HasMaxLength(500);

                 tb.Property(p => p.Precio)
                .HasPrecision(18, 2);

                tb.Property(p => p.Stock)
                .IsRequired()
                .HasDefaultValue(0); // El stock no puede ser negativo ni nulo
            });

            //USUARIO
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(c => c.IDUsuario);
                tb.Property(c => c.IDUsuario)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

                tb.Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

                 tb.Property(u => u.Password)
                .HasMaxLength(255)
                .IsRequired();
                
            });
            //PERSONA
            modelBuilder.Entity<Persona>(tb =>
            {
                tb.HasKey(c => c.IDPersona);
                tb.Property(c => c.IDPersona)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                 tb.Property(p => p.Nombre)
                .HasMaxLength(50)
                .IsRequired();

                tb.Property(p => p.Apellido)
                .HasMaxLength(50)
                .IsRequired();

                tb.Property(p => p.NroCelular)
                .HasMaxLength(20);
            });
            //CARRITO
            modelBuilder.Entity<Carrito>(tb =>
            {
                tb.HasKey(c => c.IDCarrito);
                tb.Property(c => c.IDCarrito)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();
            });
            
            //DETALLESCARRITO
            modelBuilder.Entity<DetallesCarrito>(tb =>
            {
                tb.HasKey(c => c.IDDetallesCarrito);
                tb.Property(c => c.IDDetallesCarrito)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();
            });
            #endregion


            #region Relaciones Tablas
            // Usuario y Persona (1:1)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Persona)
                .WithOne(pe => pe.Usuario)
                .HasForeignKey<Persona>(u => u.IDUsuario)
                .OnDelete(DeleteBehavior.Cascade);  // eliminacion en cascada, cuando se elimna usuario se elimina persona por su relacion 1 a 1

            //// Persona Y Usuario (1:1)
            //modelBuilder.Entity<Persona>()
            //    .HasOne(pe => pe.Usuario)
            //    .WithOne(u => u.Persona)
            //    .HasForeignKey<Persona>(u => u.IDUsuario);

            // Usuario y Carrito (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Carrito)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.IDUsuario);


            // Carrito y DetallesCarrito (1:N)
            modelBuilder.Entity<Carrito>()
                .HasMany(c => c.DetallesCarritos)
                .WithOne(dc => dc.Carrito)
                .HasForeignKey(c => c.IDCarrito)
                .OnDelete(DeleteBehavior.Cascade); ;

            // Producto y DetallesCarrito (1:N)
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.DetallesCarritos)
                .WithOne(dc => dc.Producto)
                .HasForeignKey(dc => dc.IDProducto);
            #endregion
        }
    }
}
