using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorPreco()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

                return produtosDTO;
            }
            catch (Exception e)
            {
                return Problem($"Erro ao tentar obter os produtos do banco de dados\n{e.Message}");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtoParameters)
        {
            try
            {
                var produtos = _uof.ProdutoRepository.GetProdutos(produtoParameters);

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

                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDTO;
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter os produtos do banco de dados");
            }
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.Id == id);
                if (produto == null)
                {
                    return NotFound("Produto nao encontrado.");
                }

                var produtosDTO = _mapper.Map<ProdutoDTO>(produto);
                return produtosDTO;
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter o produto do banco de dados");
            }

        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                // Atualmente não é mais necessário implementar cádigo abaixo, pois esse trecho
                // já esta implementado em [ApiController] (Desde .net core 2)
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produtoDTO);
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar criar um novo produto");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.Id)
                {
                    return BadRequest($"Não foi possível atualizar o produto id={id}");
                }

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();
                return Ok();
            }
            catch (Exception)
            {
                return Problem($"Não foi possível atualizar o produto id={id}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.Id == id);
                // Funciona apenas caso o Id seja Chave primaria, o Find primeiro procura
                // em memória, e caso não encontre, busca no banco.
                // var produto = _context.Produtos.Find(id);

                if (produto == null)
                {
                    return NotFound($"O produto id={id} não foi encontrada");
                }

                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return produtoDto;
            }
            catch (Exception)
            {
                return Problem($"Erro ao excluir o produto de id={id}");
            }
        }
    }
}
