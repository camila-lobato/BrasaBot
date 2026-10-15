using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrasaBot.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarFornecedorEmInsumo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FornecedorId",
                table: "Insumos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_FornecedorId",
                table: "Insumos",
                column: "FornecedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_Fornecedores_FornecedorId",
                table: "Insumos",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_Fornecedores_FornecedorId",
                table: "Insumos");

            migrationBuilder.DropIndex(
                name: "IX_Insumos_FornecedorId",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "FornecedorId",
                table: "Insumos");
        }
    }
}
