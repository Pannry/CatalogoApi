using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork contexto)
        {
            _uof = contexto;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            try
            {
                return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            }
            catch (Exception e)
            {
                return Problem($"Erro ao tentar obter os produtos do banco de dados\n{e.Message}");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                return _uof.ProdutoRepository.Get().ToList();
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter os produtos do banco de dados");
            }
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.Id == id);
                if (produto == null)
                {
                    return NotFound("Produto nao encontrado.");
                }
                return produto;
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter o produto do banco de dados");
            }

        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            try
            {
                // Atualmente não é mais necessário implementar cádigo abaixo, pois esse trecho
                // já esta implementado em [ApiController] (Desde .net core 2)
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar criar um novo produto");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            try
            {
                if (id != produto.Id)
                {
                    return BadRequest($"Não foi possível atualizar o produto id={id}");
                }

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
        public ActionResult<Produto> Delete(int id)
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
                return produto;
            }
            catch (Exception)
            {
                return Problem($"Erro ao excluir o produto de id={id}");
            }
        }
    }
}
