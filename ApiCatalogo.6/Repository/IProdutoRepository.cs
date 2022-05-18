using ApiCatalogo._6.Models;
using ApiCatalogo._6.Pagination;

namespace ApiCatalogo._6.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameter);
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
    }
}
