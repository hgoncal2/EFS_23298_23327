using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    /// <inheritdoc />
    public partial class criadoUserVM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "UtilizadoresViewModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "UtilizadoresViewModel");
        }
    }
}
