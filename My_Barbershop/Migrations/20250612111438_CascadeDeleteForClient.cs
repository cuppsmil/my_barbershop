using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Barbershop.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteForClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "appointment_clientphone_fkey",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "barberclient_clientphone_fkey",
                table: "barberclient");

            migrationBuilder.AddForeignKey(
                name: "appointment_clientphone_fkey",
                table: "appointment",
                column: "clientphone",
                principalTable: "client",
                principalColumn: "client_phnum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "barberclient_clientphone_fkey",
                table: "barberclient",
                column: "clientphone",
                principalTable: "client",
                principalColumn: "client_phnum",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "appointment_clientphone_fkey",
                table: "appointment");

            migrationBuilder.DropForeignKey(
                name: "barberclient_clientphone_fkey",
                table: "barberclient");

            migrationBuilder.AddForeignKey(
                name: "appointment_clientphone_fkey",
                table: "appointment",
                column: "clientphone",
                principalTable: "client",
                principalColumn: "client_phnum");

            migrationBuilder.AddForeignKey(
                name: "barberclient_clientphone_fkey",
                table: "barberclient",
                column: "clientphone",
                principalTable: "client",
                principalColumn: "client_phnum");
        }
    }
}
