using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalogo6.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Preco = table.Column<float>(type: "REAL", precision: 10, scale: 2, nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Estoque = table.Column<float>(type: "REAL", nullable: false, defaultValue: 1f),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "ImagemUrl", "Nome" },
                values: new object[] { 1, "material.jpg", "Material Escolar" });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "ImagemUrl", "Nome" },
                values: new object[] { 2, "eletronicos.jpg", "Eletrônicos" });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "ImagemUrl", "Nome" },
                values: new object[] { 3, "acessorios.jpg", "Acessórios" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos",
                column: "CategoriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
