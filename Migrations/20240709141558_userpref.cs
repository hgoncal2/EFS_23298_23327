using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    /// <inheritdoc />
    public partial class userpref : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userPrefsAnfId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPrefsAnf",
                columns: table => new
                {
                    UserPrefId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnfId = table.Column<int>(type: "int", nullable: false),
                    mostrarCanceladas = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrefsAnf", x => x.UserPrefId);
                });

            migrationBuilder.CreateTable(
                name: "UserPrefAnfCores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPrefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrefAnfCores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPrefAnfCores_UserPrefsAnf_UserPrefId",
                        column: x => x.UserPrefId,
                        principalTable: "UserPrefsAnf",
                        principalColumn: "UserPrefId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_userPrefsAnfId",
                table: "AspNetUsers",
                column: "userPrefsAnfId",
                unique: true,
                filter: "[userPrefsAnfId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrefAnfCores_UserPrefId",
                table: "UserPrefAnfCores",
                column: "UserPrefId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserPrefsAnf_userPrefsAnfId",
                table: "AspNetUsers",
                column: "userPrefsAnfId",
                principalTable: "UserPrefsAnf",
                principalColumn: "UserPrefId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserPrefsAnf_userPrefsAnfId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserPrefAnfCores");

            migrationBuilder.DropTable(
                name: "UserPrefsAnf");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_userPrefsAnfId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userPrefsAnfId",
                table: "AspNetUsers");
        }
    }
}
