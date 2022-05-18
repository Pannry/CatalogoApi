using ApiCatalogo._6.Validations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo._6.Models;

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

        CategoriaId = id;        
        Nome = nome;
        ImagemUrl = imagemUrl;
    }

    [Key]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80)]
    [PrimeiraLetraUppercase]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    // Informa que uma categoria possui uma coleção de produtos
    public ICollection<Produto> Produtos { get; set; }
}
