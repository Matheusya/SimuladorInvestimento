using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimuladorInvestimento.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaSonhos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ativos");

            migrationBuilder.CreateTable(
                name: "Sonhos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeObjetivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValorObjetivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AporteMensal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxaJurosMensal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sonhos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sonhos");

            migrationBuilder.CreateTable(
                name: "Ativos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NomeEmpresa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrecoAtual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ativos", x => x.Id);
                });
        }
    }
}
