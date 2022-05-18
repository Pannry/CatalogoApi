namespace ApiCatalogo._6.Repository
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        void Commit();
    }
}
