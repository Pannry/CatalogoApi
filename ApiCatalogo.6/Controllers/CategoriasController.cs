using ApiCatalogo._6.Context;
using ApiCatalogo._6.DTOs;
using ApiCatalogo._6.Models;
using ApiCatalogo._6.Pagination;
using ApiCatalogo._6.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ApiCatalogo._6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController: ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = _uof.CategoriaRepository.GetCategorias(categoriasParameters);
                
                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious,
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação");
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);
            if (categoria == null)
                return NotFound("Categoria não encontrada");

            return _mapper.Map<CategoriaDTO>(categoria);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategiriasProdutos()
        {
            return _mapper.Map<List<CategoriaDTO>>(_uof.CategoriaRepository.GetCategoriasProdutos().ToList());
        }

        [HttpPost]
        public ActionResult Post(CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (categoria == null)
                return BadRequest();

            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            // CreatedAtRouteResult se o produto for cadastrado no banco, retorne os valores
            // cadastrados pela rota onde possui o GET com a propriedade Name = "ObterProduto"
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, CategoriaDTO categoriaDto)
        {

            if (id != categoriaDto.CategoriaId)
                return BadRequest();

            // Informa que o objeto produto possui valores modificados e salva as informações
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoriaDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria is null)
                return NotFound("Produto não encontrado");

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }
    }
}
