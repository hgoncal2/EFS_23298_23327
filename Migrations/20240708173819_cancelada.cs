using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    /// <inheritdoc />
    public partial class cancelada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "cancelada",
                table: "Reservas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancelada",
                table: "Reservas");
        }
    }
}
