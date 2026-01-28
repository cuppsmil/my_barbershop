using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Barbershop.Migrations
{
    /// <inheritdoc />
    public partial class AddKeysForBarberClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "barberclient_barbpass_fkey",
                table: "barberclient");

            migrationBuilder.DropIndex(
                name: "IX_barberclient_barbpass",
                table: "barberclient");

            migrationBuilder.RenameTable(
                name: "barberclient",
                newName: "Barberclients");

            migrationBuilder.RenameIndex(
                name: "IX_barberclient_clientphone",
                table: "Barberclients",
                newName: "IX_Barberclients_clientphone");

            migrationBuilder.AlterColumn<long>(
                name: "clientphone",
                table: "Barberclients",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "barbpass",
                table: "Barberclients",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Barberclients",
                table: "Barberclients",
                columns: new[] { "barbpass", "clientphone" });

            migrationBuilder.AddForeignKey(
                name: "barberclient_barbpass_fkey",
                table: "Barberclients",
                column: "barbpass",
                principalTable: "barber",
                principalColumn: "barber_passport",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "barberclient_barbpass_fkey",
                table: "Barberclients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Barberclients",
                table: "Barberclients");

            migrationBuilder.RenameTable(
                name: "Barberclients",
                newName: "barberclient");

            migrationBuilder.RenameIndex(
                name: "IX_Barberclients_clientphone",
                table: "barberclient",
                newName: "IX_barberclient_clientphone");

            migrationBuilder.AlterColumn<long>(
                name: "clientphone",
                table: "barberclient",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "barbpass",
                table: "barberclient",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_barberclient_barbpass",
                table: "barberclient",
                column: "barbpass");

            migrationBuilder.AddForeignKey(
                name: "barberclient_barbpass_fkey",
                table: "barberclient",
                column: "barbpass",
                principalTable: "barber",
                principalColumn: "barber_passport");
        }
    }
}
