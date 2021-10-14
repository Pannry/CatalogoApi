using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Models
{
    // A relação entre Categoria e Produtos é de uma Categoria para muitos produtos
    [Table("Categorias")]
    public class Categoria
    {
        // Definindo a inicialização da coleção
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }

        public Categoria(int id, string nome, string imagemUrl)
        {
            Produtos = new Collection<Produto>();
            Id = id;
            Nome = nome;
            ImagemUrl = imagemUrl;
        }

        // Aparentemente o AutoIncrement está bugado (SQLite + efcore)
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string ImagemUrl { get; set; }

        // Informa que uma categoria possui uma coleção de produtos
        public ICollection<Produto> Produtos { get; set; }
    }
}
