using Catalogo6.Domain.Entities;
using Catalogo6.Domain.Interfaces;
using Catalogo6.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogo6.Infrastructure.Repositories
{
    /*
     ICategoriaRepository e IProdutoRepository servem para além do contrato, 
     adicionar na injeção de dependências.
     services.AddScoped<ICategoriaRepository, CategoriaRepository>();
     */
    public class CategoriaRepository : ICategoriaRepository
    {
        private ApplicationDbContext _categoryContext;
        public CategoriaRepository(ApplicationDbContext context)
        {
            _categoryContext = context;
        }

        public async Task<Categoria> CreateAsync(Categoria category)
        {
            _categoryContext.Add(category);
            await _categoryContext.SaveChangesAsync();
            return category;
        }

        public async Task<Categoria> GetByIdAsync(int? id)
        {
            return await _categoryContext.Categorias.FindAsync(id);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            return await _categoryContext.Categorias.ToListAsync();
        }

        public async Task<Categoria> RemoveAsync(Categoria category)
        {
            _categoryContext.Remove(category);
            await _categoryContext.SaveChangesAsync();
            return category;
        }

        public async Task<Categoria> UpdateAsync(Categoria category)
        {
            _categoryContext.Update(category);
            await _categoryContext.SaveChangesAsync();
            return category;
        }
    }
}
