using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class temasDeleted2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Temas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Temas");
        }
    }
}
