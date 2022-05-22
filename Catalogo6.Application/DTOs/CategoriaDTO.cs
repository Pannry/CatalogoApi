using System.ComponentModel.DataAnnotations;

namespace Catalogo6.Application.DTOs
{
    /* Nesse momento, categoriaDTO não possui nenhuma relação com Categoria */
    public class CategoriaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome da categoria")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o nome da imagem")]
        [MinLength(5)]
        [MaxLength(250)]
        public string ImagemUrl { get; set; }
    }
}
