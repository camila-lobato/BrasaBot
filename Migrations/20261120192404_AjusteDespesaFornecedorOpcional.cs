using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrasaBot.Migrations
{
    /// <inheritdoc />
    public partial class AjusteDespesaFornecedorOpcional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Despesas_Fornecedores_FornecedorId",
                table: "Despesas");

            migrationBuilder.AlterColumn<int>(
                name: "FornecedorId",
                table: "Despesas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Despesas_Fornecedores_FornecedorId",
                table: "Despesas",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Despesas_Fornecedores_FornecedorId",
                table: "Despesas");

            migrationBuilder.AlterColumn<int>(
                name: "FornecedorId",
                table: "Despesas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Despesas_Fornecedores_FornecedorId",
                table: "Despesas",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
