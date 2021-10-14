using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        public Produto()
        {
        }

        public Produto(int id, string nome, string descricao, decimal preco, string imagemUrl, float estoque, DateTime dataCadastro, int categoriaId)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            ImagemUrl = imagemUrl;
            Estoque = estoque;
            DataCadastro = dataCadastro;
            CategoriaId = categoriaId;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string Descricao { get; set; }
        
        [Required]
        public decimal Preco { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string ImagemUrl { get; set; }
        
        public float Estoque { get; set; }
        
        public DateTime DataCadastro { get; set; }

        // Informando que um produto está relacionado com uma categoria
        public Categoria Categoria { get; set; }
        
        public int CategoriaId { get; set; }
    }
}
