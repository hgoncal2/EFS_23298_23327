using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class usersDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Utilizadores",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Utilizadores");
        }
    }
}
