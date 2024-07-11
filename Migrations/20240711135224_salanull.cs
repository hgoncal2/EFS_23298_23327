using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    /// <inheritdoc />
    public partial class salanull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Salas_SalaId",
                table: "Reservas");

            migrationBuilder.AlterColumn<int>(
                name: "SalaId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Salas_SalaId",
                table: "Reservas",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "SalaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Salas_SalaId",
                table: "Reservas");

            migrationBuilder.AlterColumn<int>(
                name: "SalaId",
                table: "Reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Salas_SalaId",
                table: "Reservas",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "SalaId");
        }
    }
}
