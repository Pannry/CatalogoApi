using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Filters;
using APICatalogo.Repository;
using APICatalogo.Pagination;
using APICatalogo.DTOs;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork contexto, IConfiguration configuration, IMapper mapper)
        {
            _uof = contexto;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("config")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public string Config()
        {
            // Exemplo de exception global
            // throw new Exception("Erro Customizado");

            // Exemplo da utilização do arquivo de configuração
            return _configuration["ConexaoSqlite:SqliteConnectionString"];
        }

        // Exemplo de ModelBind para injeção de dependência, util para quando apenas um método
        // precisar do serviço
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos([FromServices]AppDbContext exemploDeBindServiceContext)
        {
            try
            {
                return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
                // return exemploDeBindServiceContext.Categorias.Include(x => x.Produtos).ToList();
                // return _context.Categorias.Include(x => x.Produtos).ToList();
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter os produtos das categorias do banco de dados");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var produtos = _uof.CategoriaRepository.GetCategorias(categoriasParameters);
                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious,
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                var produtosDTO = _mapper.Map<List<CategoriaDTO>>(produtos);
                return produtosDTO;
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter as categorias do banco de dados");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(p => p.Id == id);
                if (categoria == null)
                {
                    return NotFound($"A categoria id={id} não foi encontrada");
                }
                return categoria;
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter a categoria do banco de dados");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();
                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar criar uma nova categoria");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.Id)
                {
                    return BadRequest($"Não foi possível atualizar a categoria id={id}");
                }

                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
            }
            catch (Exception)
            {
                return Problem($"Não foi possível atualizar a categoria id={id}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(p => p.Id == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria id={id} não foi encontrada");
                }

                _uof.CategoriaRepository.Delete(categoria);
                _uof.Commit();
                return categoria;
            }
            catch (Exception)
            {
                return Problem($"Erro ao excluir a categoria de id={id}");
            }
        }
    }
}
