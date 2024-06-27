using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class listaFotos1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "TemaID",
                table: "Fotos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_TemaID",
                table: "Fotos",
                column: "TemaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotos_Temas_TemaID",
                table: "Fotos",
                column: "TemaID",
                principalTable: "Temas",
                principalColumn: "TemaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotos_Temas_TemaID",
                table: "Fotos");

            migrationBuilder.DropIndex(
                name: "IX_Fotos_TemaID",
                table: "Fotos");

            migrationBuilder.DropColumn(
                name: "TemaID",
                table: "Fotos");

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
    }
}
