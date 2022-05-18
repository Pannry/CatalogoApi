using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalogo._6.Models;

[Table("Produtos")]
public class Produto
{
    public Produto() { }

    public Produto(int id, string nome, string descricao, float preco, string imagemUrl, float estoque, DateTime dataCadastro, int categoriaId)
    {
        ProdutoId = id;
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        ImagemUrl = imagemUrl;
        Estoque = estoque;
        DataCadastro = dataCadastro;
        CategoriaId = categoriaId;
    }

    [Key]
    public int ProdutoId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }
    
    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }

    // SQLite não se garante bem com decimal
    // [Column(TypeName ="decimal(8,2)")] // Para o tipo decimal
    public float Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    public float Estoque { get; set; }
    
    public DateTime DataCadastro { get; set; }

    // Informando que um produto está relacionado com uma categoria
    public int CategoriaId { get; set; }

    [JsonIgnore] // Ignora a propriedade na hora do CRUD
    public Categoria? Categoria { get; set; }
}
