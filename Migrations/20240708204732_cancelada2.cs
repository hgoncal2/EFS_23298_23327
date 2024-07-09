using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    /// <inheritdoc />
    public partial class cancelada2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cancelada",
                table: "Reservas",
                newName: "Cancelada");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cancelada",
                table: "Reservas",
                newName: "cancelada");
        }
    }
}
