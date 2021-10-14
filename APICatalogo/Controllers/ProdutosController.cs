using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                return _context.Produtos.AsNoTracking().ToList();
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
                var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.Id == id);
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

                _context.Produtos.Add(produto);
                _context.SaveChanges();
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

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();
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
                var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
                // Funciona apenas caso o Id seja Chave primaria, o Find primeiro procura
                // em memória, e caso não encontre, busca no banco.
                // var produto = _context.Produtos.Find(id);

                if (produto == null)
                {
                    return NotFound($"O produto id={id} não foi encontrada");
                    return NotFound();
                }

                _context.Produtos.Remove(produto);
                _context.SaveChanges();
                return produto;
            }
            catch (Exception)
            {
                return Problem($"Erro ao excluir a categoria de id={id}");
            }
        }
    }
}
