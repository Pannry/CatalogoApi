using ApiCatalogo._6.Models;
using ApiCatalogo._6.Pagination;

namespace ApiCatalogo._6.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
