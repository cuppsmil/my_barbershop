using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Barbershop.Migrations
{
    /// <inheritdoc />
    public partial class BarberClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Barberclients",
                table: "Barberclients");

            migrationBuilder.RenameTable(
                name: "Barberclients",
                newName: "BarberClient");

            migrationBuilder.RenameIndex(
                name: "IX_Barberclients_clientphone",
                table: "BarberClient",
                newName: "IX_BarberClient_clientphone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BarberClient",
                table: "BarberClient",
                columns: new[] { "barbpass", "clientphone" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BarberClient",
                table: "BarberClient");

            migrationBuilder.RenameTable(
                name: "BarberClient",
                newName: "Barberclients");

            migrationBuilder.RenameIndex(
                name: "IX_BarberClient_clientphone",
                table: "Barberclients",
                newName: "IX_Barberclients_clientphone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Barberclients",
                table: "Barberclients",
                columns: new[] { "barbpass", "clientphone" });
        }
    }
}
