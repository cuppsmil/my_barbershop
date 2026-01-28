using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Barbershop.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeForServiceAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "appointment_servicename_fkey",
                table: "appointment");

            migrationBuilder.AddForeignKey(
                name: "appointment_servicename_fkey",
                table: "appointment",
                column: "servicename",
                principalTable: "service",
                principalColumn: "serv_name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "appointment_servicename_fkey",
                table: "appointment");

            migrationBuilder.AddForeignKey(
                name: "appointment_servicename_fkey",
                table: "appointment",
                column: "servicename",
                principalTable: "service",
                principalColumn: "serv_name");
        }
    }
}
