namespace ApiCatalogo._6.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public float Preco { get; set; }
        public string ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
