using AutoMapper;
using Catalogo6.Application.DTOs;
using Catalogo6.Domain.Entities;

namespace Catalogo6.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
        }
    }
}
