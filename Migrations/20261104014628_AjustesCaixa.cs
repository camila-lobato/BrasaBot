using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrasaBot.Migrations
{
    /// <inheritdoc />
    public partial class AjustesCaixa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "MovimentacoesCaixa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "Caixas",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesCaixa_PedidoId",
                table: "MovimentacoesCaixa",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesCaixa_Pedidos_PedidoId",
                table: "MovimentacoesCaixa",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesCaixa_Pedidos_PedidoId",
                table: "MovimentacoesCaixa");

            migrationBuilder.DropIndex(
                name: "IX_MovimentacoesCaixa_PedidoId",
                table: "MovimentacoesCaixa");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "MovimentacoesCaixa");

            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "Caixas");
        }
    }
}
