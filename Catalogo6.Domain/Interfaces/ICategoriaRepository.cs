using Catalogo6.Domain.Entities;

namespace Catalogo6.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetCategoriasAsync();
        Task<Categoria> GetByIdAsync(int? id);
        Task<Categoria> CreateAsync(Categoria categoria);
        Task<Categoria> UpdateAsync(Categoria categoria);
        Task<Categoria> RemoveAsync(Categoria categoria);
    }
}
