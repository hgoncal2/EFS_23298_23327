using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class nomeFoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Caminho",
                table: "Fotos",
                newName: "Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Fotos",
                newName: "Caminho");
        }
    }
}
