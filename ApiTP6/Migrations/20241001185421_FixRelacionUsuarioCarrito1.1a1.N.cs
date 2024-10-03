using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTP6.Migrations
{
    /// <inheritdoc />
    public partial class FixRelacionUsuarioCarrito11a1N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carritos_IDUsuario",
                table: "Carritos");

            migrationBuilder.CreateIndex(
                name: "IX_Carritos_IDUsuario",
                table: "Carritos",
                column: "IDUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carritos_IDUsuario",
                table: "Carritos");

            migrationBuilder.CreateIndex(
                name: "IX_Carritos_IDUsuario",
                table: "Carritos",
                column: "IDUsuario",
                unique: true);
        }
    }
}
