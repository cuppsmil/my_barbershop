using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Barbershop.Migrations
{
    /// <inheritdoc />
    public partial class DeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "client");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "administrator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "client",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "administrator",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "NOW()");
        }
    }
}
