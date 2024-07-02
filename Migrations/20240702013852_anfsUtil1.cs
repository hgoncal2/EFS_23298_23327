using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    /// <inheritdoc />
    public partial class anfsUtil1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Salas_SalasSalaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Salas_AspNetUsers_AnfitrioesId",
                table: "Salas");

            migrationBuilder.DropIndex(
                name: "IX_Salas_AnfitrioesId",
                table: "Salas");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SalasSalaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AnfitrioesId",
                table: "Salas");

            migrationBuilder.DropColumn(
                name: "SalasSalaId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AnfitrioesSalas",
                columns: table => new
                {
                    ListaAnfitrioesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListaSalasSalaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnfitrioesSalas", x => new { x.ListaAnfitrioesId, x.ListaSalasSalaId });
                    table.ForeignKey(
                        name: "FK_AnfitrioesSalas_AspNetUsers_ListaAnfitrioesId",
                        column: x => x.ListaAnfitrioesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnfitrioesSalas_Salas_ListaSalasSalaId",
                        column: x => x.ListaSalasSalaId,
                        principalTable: "Salas",
                        principalColumn: "SalaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnfitrioesSalas_ListaSalasSalaId",
                table: "AnfitrioesSalas",
                column: "ListaSalasSalaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnfitrioesSalas");

            migrationBuilder.AddColumn<string>(
                name: "AnfitrioesId",
                table: "Salas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalasSalaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salas_AnfitrioesId",
                table: "Salas",
                column: "AnfitrioesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SalasSalaId",
                table: "AspNetUsers",
                column: "SalasSalaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Salas_SalasSalaId",
                table: "AspNetUsers",
                column: "SalasSalaId",
                principalTable: "Salas",
                principalColumn: "SalaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salas_AspNetUsers_AnfitrioesId",
                table: "Salas",
                column: "AnfitrioesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
