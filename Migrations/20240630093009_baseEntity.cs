using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class baseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "deleted",
                table: "Fotos",
                newName: "Deleted");

            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "Temas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "Salas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Salas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Reservas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "Fotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Fotos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "Temas");

            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "Salas");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Salas");

            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "Fotos");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Fotos");

            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "Fotos",
                newName: "deleted");
        }
    }
}
