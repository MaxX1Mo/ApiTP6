﻿// <auto-generated />
using System;
using ApiTP6.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiTP6.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241003162150_AgregarRoles")]
    partial class AgregarRoles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ApiTP6.Models.Carrito", b =>
                {
                    b.Property<int>("IDCarrito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDCarrito"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("IDUsuario")
                        .HasColumnType("int");

                    b.HasKey("IDCarrito");

                    b.HasIndex("IDUsuario");

                    b.ToTable("Carritos");
                });

            modelBuilder.Entity("ApiTP6.Models.DetallesCarrito", b =>
                {
                    b.Property<int>("IDDetallesCarrito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDDetallesCarrito"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("IDCarrito")
                        .HasColumnType("int");

                    b.Property<int>("IDProducto")
                        .HasColumnType("int");

                    b.HasKey("IDDetallesCarrito");

                    b.HasIndex("IDCarrito");

                    b.HasIndex("IDProducto");

                    b.ToTable("DetallesCarritos");
                });

            modelBuilder.Entity("ApiTP6.Models.Persona", b =>
                {
                    b.Property<int>("IDPersona")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDPersona"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IDUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NroCelular")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("IDPersona");

                    b.HasIndex("IDUsuario")
                        .IsUnique();

                    b.ToTable("Personas");
                });

            modelBuilder.Entity("ApiTP6.Models.Producto", b =>
                {
                    b.Property<int>("IDProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDProducto"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Imagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreProducto")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Precio")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("IDProducto");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("ApiTP6.Models.Usuario", b =>
                {
                    b.Property<int>("IDUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDUsuario"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("IDPersona")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Rol")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IDUsuario");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ApiTP6.Models.Carrito", b =>
                {
                    b.HasOne("ApiTP6.Models.Usuario", "Usuario")
                        .WithMany("Carrito")
                        .HasForeignKey("IDUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ApiTP6.Models.DetallesCarrito", b =>
                {
                    b.HasOne("ApiTP6.Models.Carrito", "Carrito")
                        .WithMany("DetallesCarritos")
                        .HasForeignKey("IDCarrito")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiTP6.Models.Producto", "Producto")
                        .WithMany("DetallesCarritos")
                        .HasForeignKey("IDProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carrito");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("ApiTP6.Models.Persona", b =>
                {
                    b.HasOne("ApiTP6.Models.Usuario", "Usuario")
                        .WithOne("Persona")
                        .HasForeignKey("ApiTP6.Models.Persona", "IDUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ApiTP6.Models.Carrito", b =>
                {
                    b.Navigation("DetallesCarritos");
                });

            modelBuilder.Entity("ApiTP6.Models.Producto", b =>
                {
                    b.Navigation("DetallesCarritos");
                });

            modelBuilder.Entity("ApiTP6.Models.Usuario", b =>
                {
                    b.Navigation("Carrito");

                    b.Navigation("Persona")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
