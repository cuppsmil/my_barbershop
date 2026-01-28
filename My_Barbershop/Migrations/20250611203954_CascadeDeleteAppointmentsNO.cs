using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Barbershop.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteAppointmentsNO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "appointment_barberpassport_fkey",
                table: "appointment");

            migrationBuilder.AddForeignKey(
                name: "appointment_barberpassport_fkey",
                table: "appointment",
                column: "barberpassport",
                principalTable: "barber",
                principalColumn: "barber_passport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "appointment_barberpassport_fkey",
                table: "appointment");

            migrationBuilder.AddForeignKey(
                name: "appointment_barberpassport_fkey",
                table: "appointment",
                column: "barberpassport",
                principalTable: "barber",
                principalColumn: "barber_passport",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
