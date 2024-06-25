using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class updatesTbls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Utilizadores_UtilizadorID",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Temas_Fotos_FotoID",
                table: "Temas");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Utilizadores",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "FotoID",
                table: "Temas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadorID",
                table: "Reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Utilizadores_UtilizadorID",
                table: "Reservas",
                column: "UtilizadorID",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Temas_Fotos_FotoID",
                table: "Temas",
                column: "FotoID",
                principalTable: "Fotos",
                principalColumn: "FotoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Utilizadores_UtilizadorID",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Temas_Fotos_FotoID",
                table: "Temas");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AlterColumn<int>(
                name: "FotoID",
                table: "Temas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadorID",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Utilizadores_UtilizadorID",
                table: "Reservas",
                column: "UtilizadorID",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Temas_Fotos_FotoID",
                table: "Temas",
                column: "FotoID",
                principalTable: "Fotos",
                principalColumn: "FotoID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
