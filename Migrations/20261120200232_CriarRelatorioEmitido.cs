using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrasaBot.Migrations
{
    /// <inheritdoc />
    public partial class CriarRelatorioEmitido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelatoriosEmitidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TipoRelatorio = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Filtro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInicial = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatoriosEmitidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatoriosEmitidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RelatoriosEmitidos_UsuarioId",
                table: "RelatoriosEmitidos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatoriosEmitidos");
        }
    }
}
