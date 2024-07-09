using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    /// <inheritdoc />
    public partial class cancelData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCancel",
                table: "Reservas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCancel",
                table: "Reservas");
        }
    }
}
