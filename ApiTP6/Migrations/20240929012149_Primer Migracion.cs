using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTP6.Migrations
{
    /// <inheritdoc />
    public partial class PrimerMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    IDPersona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NroCelular = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.IDPersona);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IDProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IDProducto);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IDUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IDPersona = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IDUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Personas_IDPersona",
                        column: x => x.IDPersona,
                        principalTable: "Personas",
                        principalColumn: "IDPersona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carritos",
                columns: table => new
                {
                    IDCarrito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carritos", x => x.IDCarrito);
                    table.ForeignKey(
                        name: "FK_Carritos_Usuarios_IDUsuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IDUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesCarritos",
                columns: table => new
                {
                    IDDetallesCarrito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    IDProducto = table.Column<int>(type: "int", nullable: false),
                    IDCarrito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesCarritos", x => x.IDDetallesCarrito);
                    table.ForeignKey(
                        name: "FK_DetallesCarritos_Carritos_IDCarrito",
                        column: x => x.IDCarrito,
                        principalTable: "Carritos",
                        principalColumn: "IDCarrito",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesCarritos_Productos_IDProducto",
                        column: x => x.IDProducto,
                        principalTable: "Productos",
                        principalColumn: "IDProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carritos_IDUsuario",
                table: "Carritos",
                column: "IDUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCarritos_IDCarrito",
                table: "DetallesCarritos",
                column: "IDCarrito");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCarritos_IDProducto",
                table: "DetallesCarritos",
                column: "IDProducto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IDPersona",
                table: "Usuarios",
                column: "IDPersona",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesCarritos");

            migrationBuilder.DropTable(
                name: "Carritos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Personas");
        }
    }
}
