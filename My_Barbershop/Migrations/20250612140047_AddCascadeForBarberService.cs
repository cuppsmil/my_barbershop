using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Barbershop.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeForBarberService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "barberservice_barbpass_fkey",
                table: "barberservice");

            migrationBuilder.DropForeignKey(
                name: "barberservice_servname_fkey",
                table: "barberservice");

            migrationBuilder.DropIndex(
                name: "IX_barberservice_barbpass",
                table: "barberservice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BarberClient",
                table: "BarberClient");

            migrationBuilder.RenameTable(
                name: "BarberClient",
                newName: "barberclient");

            migrationBuilder.RenameIndex(
                name: "IX_BarberClient_clientphone",
                table: "barberclient",
                newName: "IX_barberclient_clientphone");

            migrationBuilder.AlterColumn<string>(
                name: "cl_fio",
                table: "client",
                type: "character varying(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "servname",
                table: "barberservice",
                type: "character varying(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "barbpass",
                table: "barberservice",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_barberservice",
                table: "barberservice",
                columns: new[] { "barbpass", "servname" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_barberclient",
                table: "barberclient",
                columns: new[] { "barbpass", "clientphone" });

            migrationBuilder.AddForeignKey(
                name: "barberservice_barbpass_fkey",
                table: "barberservice",
                column: "barbpass",
                principalTable: "barber",
                principalColumn: "barber_passport",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "barberservice_servname_fkey",
                table: "barberservice",
                column: "servname",
                principalTable: "service",
                principalColumn: "serv_name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "barberservice_barbpass_fkey",
                table: "barberservice");

            migrationBuilder.DropForeignKey(
                name: "barberservice_servname_fkey",
                table: "barberservice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_barberservice",
                table: "barberservice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_barberclient",
                table: "barberclient");

            migrationBuilder.RenameTable(
                name: "barberclient",
                newName: "BarberClient");

            migrationBuilder.RenameIndex(
                name: "IX_barberclient_clientphone",
                table: "BarberClient",
                newName: "IX_BarberClient_clientphone");

            migrationBuilder.AlterColumn<string>(
                name: "cl_fio",
                table: "client",
                type: "character varying(254)",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254);

            migrationBuilder.AlterColumn<string>(
                name: "servname",
                table: "barberservice",
                type: "character varying(254)",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254);

            migrationBuilder.AlterColumn<long>(
                name: "barbpass",
                table: "barberservice",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BarberClient",
                table: "BarberClient",
                columns: new[] { "barbpass", "clientphone" });

            migrationBuilder.CreateIndex(
                name: "IX_barberservice_barbpass",
                table: "barberservice",
                column: "barbpass");

            migrationBuilder.AddForeignKey(
                name: "barberservice_barbpass_fkey",
                table: "barberservice",
                column: "barbpass",
                principalTable: "barber",
                principalColumn: "barber_passport");

            migrationBuilder.AddForeignKey(
                name: "barberservice_servname_fkey",
                table: "barberservice",
                column: "servname",
                principalTable: "service",
                principalColumn: "serv_name");
        }
    }
}
