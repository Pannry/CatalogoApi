using ApiCatalogo._6.Context;
using ApiCatalogo._6.Models;
using ApiCatalogo._6.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo._6.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            return PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.Nome), 
                        categoriasParameters.PageNumber, categoriasParameters.PageSize);
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(c => c.Produtos);
        }
    }
}
