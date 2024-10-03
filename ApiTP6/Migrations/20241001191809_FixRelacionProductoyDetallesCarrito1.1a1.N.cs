using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTP6.Migrations
{
    /// <inheritdoc />
    public partial class FixRelacionProductoyDetallesCarrito11a1N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DetallesCarritos_IDProducto",
                table: "DetallesCarritos");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCarritos_IDProducto",
                table: "DetallesCarritos",
                column: "IDProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DetallesCarritos_IDProducto",
                table: "DetallesCarritos");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCarritos_IDProducto",
                table: "DetallesCarritos",
                column: "IDProducto",
                unique: true);
        }
    }
}
