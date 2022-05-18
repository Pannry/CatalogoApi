using ApiCatalogo._6.Context;

namespace ApiCatalogo._6.Repository
{
    /*
    1a: Implementação de métodos genéricos no IRepository;
    2a: Implementação concreta do Repository, essa classe será herdada por todas as classes que realizam 
            CRUD no banco de dados;
    3a: Implementações do IProdutoRepository e ICategoriaRepository e suas respectivas classes concretas.
    4a: As classes concretas Produto e Categoria herdam da classe generica repository para as operações 
            CRUD basicas e implementam as interfaces que possuem métodos especificos do Modelo;
     */

    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepo;
        private CategoriaRepository _categoriaRepo;
        private AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public async Task Commit()
        {
            _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
