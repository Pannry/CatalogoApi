using Catalogo6.Domain.Validation;

namespace Catalogo6.Domain.Entities
{
    public sealed class Categoria : Entity
    {
        public Categoria(string nome, string imagemUrl)
        {
            ValidateDomain(nome, imagemUrl);
        }

        public Categoria(int id, string nome, string imagemUrl)
        {
            DomainExceptionValidation.When(id < 0, "Valor de Id inválido");
            Id = id;
            ValidateDomain(nome, imagemUrl);
        }

        public string? Nome { get; private set; }
        public string? ImagemUrl { get; private set; }
        public ICollection<Produto> Produtos { get; set; }

        private void ValidateDomain(string nome, string imagemUrl)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(nome),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(string.IsNullOrEmpty(imagemUrl),
                "Nome da imagem inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(nome.Length < 3,
                "O Nome deve ter no mínimo de 3 caracteres");

            DomainExceptionValidation.When(imagemUrl.Length < 5,
                "O Nome da imagem deve ter no mínimo de 5 caracteres");

            Nome = nome;
            ImagemUrl = imagemUrl;
        }
    }
}
