using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23306.Migrations
{
    /// <inheritdoc />
    public partial class loginVM2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnfitrioesSalas_Utilizadores_ListaAnfitrioesUtilizadorID",
                table: "AnfitrioesSalas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Utilizadores_ClientesUtilizadorID",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Utilizadores_UtilizadorID",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_ClientesUtilizadorID",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnfitrioesSalas",
                table: "AnfitrioesSalas");

            migrationBuilder.DropColumn(
                name: "ClientesUtilizadorID",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "ListaAnfitrioesUtilizadorID",
                table: "AnfitrioesSalas");

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorID",
                table: "Reservas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientesId",
                table: "Reservas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimeiroNome",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UltimoNome",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtilizadorID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ListaAnfitrioesId",
                table: "AnfitrioesSalas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnfitrioesSalas",
                table: "AnfitrioesSalas",
                columns: new[] { "ListaAnfitrioesId", "ListaSalasSalaID" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ClientesId",
                table: "Reservas",
                column: "ClientesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnfitrioesSalas_AspNetUsers_ListaAnfitrioesId",
                table: "AnfitrioesSalas",
                column: "ListaAnfitrioesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_AspNetUsers_ClientesId",
                table: "Reservas",
                column: "ClientesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_AspNetUsers_UtilizadorID",
                table: "Reservas",
                column: "UtilizadorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnfitrioesSalas_AspNetUsers_ListaAnfitrioesId",
                table: "AnfitrioesSalas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_AspNetUsers_ClientesId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_AspNetUsers_UtilizadorID",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_ClientesId",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnfitrioesSalas",
                table: "AnfitrioesSalas");

            migrationBuilder.DropColumn(
                name: "ClientesId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimeiroNome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UltimoNome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UtilizadorID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ListaAnfitrioesId",
                table: "AnfitrioesSalas");

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadorID",
                table: "Reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientesUtilizadorID",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListaAnfitrioesUtilizadorID",
                table: "AnfitrioesSalas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnfitrioesSalas",
                table: "AnfitrioesSalas",
                columns: new[] { "ListaAnfitrioesUtilizadorID", "ListaSalasSalaID" });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    UtilizadorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PrimeiroNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UltimoNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.UtilizadorID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ClientesUtilizadorID",
                table: "Reservas",
                column: "ClientesUtilizadorID");

            migrationBuilder.AddForeignKey(
                name: "FK_AnfitrioesSalas_Utilizadores_ListaAnfitrioesUtilizadorID",
                table: "AnfitrioesSalas",
                column: "ListaAnfitrioesUtilizadorID",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Utilizadores_ClientesUtilizadorID",
                table: "Reservas",
                column: "ClientesUtilizadorID",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Utilizadores_UtilizadorID",
                table: "Reservas",
                column: "UtilizadorID",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorID");
        }
    }
}
