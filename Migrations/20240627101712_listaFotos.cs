using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class listaFotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Temas_Fotos_FotoID",
                table: "Temas");

            migrationBuilder.DropIndex(
                name: "IX_Temas_FotoID",
                table: "Temas");

            migrationBuilder.DropColumn(
                name: "FotoID",
                table: "Temas");

            migrationBuilder.AddColumn<int>(
                name: "TemasTemaID",
                table: "Fotos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_TemasTemaID",
                table: "Fotos",
                column: "TemasTemaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotos_Temas_TemasTemaID",
                table: "Fotos",
                column: "TemasTemaID",
                principalTable: "Temas",
                principalColumn: "TemaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotos_Temas_TemasTemaID",
                table: "Fotos");

            migrationBuilder.DropIndex(
                name: "IX_Fotos_TemasTemaID",
                table: "Fotos");

            migrationBuilder.DropColumn(
                name: "TemasTemaID",
                table: "Fotos");

            migrationBuilder.AddColumn<int>(
                name: "FotoID",
                table: "Temas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Temas_FotoID",
                table: "Temas",
                column: "FotoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Temas_Fotos_FotoID",
                table: "Temas",
                column: "FotoID",
                principalTable: "Fotos",
                principalColumn: "FotoID");
        }
    }
}
