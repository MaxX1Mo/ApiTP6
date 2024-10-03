using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTP6.Migrations
{
    /// <inheritdoc />
    public partial class FixeandoRelaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Personas_IDPersona",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IDPersona",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_IDUsuario",
                table: "Personas",
                column: "IDUsuario",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Usuarios_IDUsuario",
                table: "Personas",
                column: "IDUsuario",
                principalTable: "Usuarios",
                principalColumn: "IDUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Usuarios_IDUsuario",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Personas_IDUsuario",
                table: "Personas");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IDPersona",
                table: "Usuarios",
                column: "IDPersona",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Personas_IDPersona",
                table: "Usuarios",
                column: "IDPersona",
                principalTable: "Personas",
                principalColumn: "IDPersona",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
