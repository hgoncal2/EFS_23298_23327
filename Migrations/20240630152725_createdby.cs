using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class createdby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CriadoPor",
                table: "Temas",
                newName: "CriadoPorUsername");

            migrationBuilder.RenameColumn(
                name: "CriadoPor",
                table: "Salas",
                newName: "CriadoPorUsername");

            migrationBuilder.RenameColumn(
                name: "CriadoPor",
                table: "Reservas",
                newName: "CriadoPorUsername");

            migrationBuilder.RenameColumn(
                name: "CriadoPor",
                table: "Fotos",
                newName: "CriadoPorUsername");

            migrationBuilder.RenameColumn(
                name: "CriadoPor",
                table: "AspNetUsers",
                newName: "CriadoPorUsername");

            migrationBuilder.AddColumn<string>(
                name: "CriadoPorOid",
                table: "Temas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CriadoPorOid",
                table: "Salas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CriadoPorOid",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CriadoPorOid",
                table: "Fotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CriadoPorOid",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriadoPorOid",
                table: "Temas");

            migrationBuilder.DropColumn(
                name: "CriadoPorOid",
                table: "Salas");

            migrationBuilder.DropColumn(
                name: "CriadoPorOid",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "CriadoPorOid",
                table: "Fotos");

            migrationBuilder.DropColumn(
                name: "CriadoPorOid",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CriadoPorUsername",
                table: "Temas",
                newName: "CriadoPor");

            migrationBuilder.RenameColumn(
                name: "CriadoPorUsername",
                table: "Salas",
                newName: "CriadoPor");

            migrationBuilder.RenameColumn(
                name: "CriadoPorUsername",
                table: "Reservas",
                newName: "CriadoPor");

            migrationBuilder.RenameColumn(
                name: "CriadoPorUsername",
                table: "Fotos",
                newName: "CriadoPor");

            migrationBuilder.RenameColumn(
                name: "CriadoPorUsername",
                table: "AspNetUsers",
                newName: "CriadoPor");
        }
    }
}
