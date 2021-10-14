using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalogo.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false)
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
                    Nome = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Preco = table.Column<decimal>(type: "TEXT", nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Estoque = table.Column<float>(type: "REAL", nullable: false),
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
                values: new object[] { 1, "http://www.example.com/imagens/1.jpg", "Bebidas" });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "ImagemUrl", "Nome" },
                values: new object[] { 2, "http://www.example.com/imagens/2.jpg", "Lanches" });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "ImagemUrl", "Nome" },
                values: new object[] { 3, "http://www.example.com/imagens/3.jpg", "Sobremesas" });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "CategoriaId", "DataCadastro", "Descricao", "Estoque", "ImagemUrl", "Nome", "Preco" },
                values: new object[] { 1, 1, new DateTime(2021, 10, 8, 9, 47, 44, 127, DateTimeKind.Local).AddTicks(44), "Refrigerante de Cola", 50f, "http://www.example.com/Imagens/coca.jpg", "Coca-cola Diet", 5.45m });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "CategoriaId", "DataCadastro", "Descricao", "Estoque", "ImagemUrl", "Nome", "Preco" },
                values: new object[] { 2, 2, new DateTime(2021, 10, 8, 9, 47, 44, 128, DateTimeKind.Local).AddTicks(3132), "Lanche de Atum com maionese", 10f, "http://www.example.com/Imagens/atum.jpg", "Lanche de Atum", 8.50m });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "CategoriaId", "DataCadastro", "Descricao", "Estoque", "ImagemUrl", "Nome", "Preco" },
                values: new object[] { 3, 3, new DateTime(2021, 10, 8, 9, 47, 44, 128, DateTimeKind.Local).AddTicks(3152), "Pudim de leite condensado 100g", 20f, "http://www.example.com/Imagens/pudim.jpg", "Pudim 100g", 6.75m });

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
