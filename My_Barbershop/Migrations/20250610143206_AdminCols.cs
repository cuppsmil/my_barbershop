using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace My_Barbershop.Migrations
{
    /// <inheritdoc />
    public partial class AdminCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "barbershop",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    address = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    name = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("barbershop_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    client_phnum = table.Column<long>(type: "bigint", nullable: false),
                    cl_fio = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    discount_card = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    card_issue_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    bonus_points = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("client_pkey", x => x.client_phnum);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    serv_name = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    durofwork = table.Column<double>(type: "double precision", nullable: true),
                    serv_price = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("service_pkey", x => x.serv_name);
                });

            migrationBuilder.CreateTable(
                name: "administrator",
                columns: table => new
                {
                    admin_passport = table.Column<long>(type: "bigint", nullable: false),
                    adm_fio = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    admin_phnumber = table.Column<long>(type: "bigint", nullable: true),
                    b_num = table.Column<long>(type: "bigint", nullable: true),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("administrator_pkey", x => x.admin_passport);
                    table.ForeignKey(
                        name: "administrator_b_num_fkey",
                        column: x => x.b_num,
                        principalTable: "barbershop",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "barber",
                columns: table => new
                {
                    barber_passport = table.Column<long>(type: "bigint", nullable: false),
                    barber_fio = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    barber_phnumber = table.Column<long>(type: "bigint", nullable: true),
                    b_num = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("barber_pkey", x => x.barber_passport);
                    table.ForeignKey(
                        name: "barber_b_num_fkey",
                        column: x => x.b_num,
                        principalTable: "barbershop",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "appointment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    barberpassport = table.Column<long>(type: "bigint", nullable: true),
                    clientphone = table.Column<long>(type: "bigint", nullable: true),
                    servicename = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    appointmentdate = table.Column<DateOnly>(type: "date", nullable: false),
                    appointmenttime = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("appointment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "appointment_barberpassport_fkey",
                        column: x => x.barberpassport,
                        principalTable: "barber",
                        principalColumn: "barber_passport");
                    table.ForeignKey(
                        name: "appointment_clientphone_fkey",
                        column: x => x.clientphone,
                        principalTable: "client",
                        principalColumn: "client_phnum");
                    table.ForeignKey(
                        name: "appointment_servicename_fkey",
                        column: x => x.servicename,
                        principalTable: "service",
                        principalColumn: "serv_name");
                });

            migrationBuilder.CreateTable(
                name: "barberclient",
                columns: table => new
                {
                    barbpass = table.Column<long>(type: "bigint", nullable: true),
                    clientphone = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "barberclient_barbpass_fkey",
                        column: x => x.barbpass,
                        principalTable: "barber",
                        principalColumn: "barber_passport");
                    table.ForeignKey(
                        name: "barberclient_clientphone_fkey",
                        column: x => x.clientphone,
                        principalTable: "client",
                        principalColumn: "client_phnum");
                });

            migrationBuilder.CreateTable(
                name: "barberservice",
                columns: table => new
                {
                    barbpass = table.Column<long>(type: "bigint", nullable: true),
                    servname = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "barberservice_barbpass_fkey",
                        column: x => x.barbpass,
                        principalTable: "barber",
                        principalColumn: "barber_passport");
                    table.ForeignKey(
                        name: "barberservice_servname_fkey",
                        column: x => x.servname,
                        principalTable: "service",
                        principalColumn: "serv_name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_administrator_b_num",
                table: "administrator",
                column: "b_num");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_barberpassport",
                table: "appointment",
                column: "barberpassport");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_clientphone",
                table: "appointment",
                column: "clientphone");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_servicename",
                table: "appointment",
                column: "servicename");

            migrationBuilder.CreateIndex(
                name: "IX_barber_b_num",
                table: "barber",
                column: "b_num");

            migrationBuilder.CreateIndex(
                name: "IX_barberclient_barbpass",
                table: "barberclient",
                column: "barbpass");

            migrationBuilder.CreateIndex(
                name: "IX_barberclient_clientphone",
                table: "barberclient",
                column: "clientphone");

            migrationBuilder.CreateIndex(
                name: "IX_barberservice_barbpass",
                table: "barberservice",
                column: "barbpass");

            migrationBuilder.CreateIndex(
                name: "IX_barberservice_servname",
                table: "barberservice",
                column: "servname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrator");

            migrationBuilder.DropTable(
                name: "appointment");

            migrationBuilder.DropTable(
                name: "barberclient");

            migrationBuilder.DropTable(
                name: "barberservice");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "barber");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "barbershop");
        }
    }
}
