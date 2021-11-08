using APICatalogo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace APICatalogo.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>().HasData(
                    new Categoria(1, "Bebidas", "http://www.example.com/imagens/1.jpg"),
                    new Categoria(2, "Lanches", "http://www.example.com/imagens/2.jpg"),
                    new Categoria(3, "Sobremesas", "http://www.example.com/imagens/3.jpg")
                );

            modelBuilder.Entity<Produto>().HasData(
                    new Produto(1, "Coca-cola Diet", "Refrigerante de Cola", 5.45f, "http://www.example.com/Imagens/coca.jpg", 50, DateTime.Now, 1),
                    new Produto(2, "Lanche de Atum", "Lanche de Atum com maionese", 8.50f, "http://www.example.com/Imagens/atum.jpg", 10, DateTime.Now, 2),
                    new Produto(3, "Pudim 100g", "Pudim de leite condensado 100g", 6.75f, "http://www.example.com/Imagens/pudim.jpg", 20, DateTime.Now, 3)
                );
        }
    }
}