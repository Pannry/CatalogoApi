using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriasController(AppDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                return _context.Categorias.Include(x => x.Produtos).ToList();
            }
            catch (System.Exception)
            {
                return Problem("Erro ao tentar obter os produtos das categorias do banco de dados");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias.AsNoTracking().ToList();
            }
            catch (System.Exception)
            {
                return Problem("Erro ao tentar obter as categorias do banco de dados");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.Id == id);
                if (categoria == null)
                {
                    return NotFound($"A categoria id={id} não foi encontrada");
                }
                return categoria;
            }
            catch (System.Exception)
            {
                return Problem("Erro ao tentar obter a categoria do banco de dados");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria);
                _context.SaveChanges();
                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
            }
            catch (System.Exception)
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

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
            }
            catch (System.Exception)
            {
                return Problem($"Não foi possível atualizar a categoria id={id}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria id={id} não foi encontrada");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
                return categoria;
            }
            catch (System.Exception)
            {
                return Problem($"Erro ao excluir a categoria de id={id}");
            }
        }
    }
}
