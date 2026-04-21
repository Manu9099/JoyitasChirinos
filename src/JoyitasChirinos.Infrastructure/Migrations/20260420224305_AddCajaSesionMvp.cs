using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyitasChirinos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCajaSesionMvp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "categorias",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "caja_sesiones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_apertura = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    monto_inicial = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    fecha_cierre = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    monto_final = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    observaciones_apertura = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    observaciones_cierre = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    abierta = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caja_sesiones", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "caja_sesiones");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "categorias",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
