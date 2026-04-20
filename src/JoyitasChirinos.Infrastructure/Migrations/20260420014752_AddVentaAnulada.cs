using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyitasChirinos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVentaAnulada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ventas_clientes_cliente_id",
                table: "ventas");

            migrationBuilder.AddColumn<bool>(
                name: "anulada",
                table: "ventas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ventas_clientes_cliente_id",
                table: "ventas",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ventas_clientes_cliente_id",
                table: "ventas");

            migrationBuilder.DropColumn(
                name: "anulada",
                table: "ventas");

            migrationBuilder.AddForeignKey(
                name: "FK_ventas_clientes_cliente_id",
                table: "ventas",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id");
        }
    }
}
