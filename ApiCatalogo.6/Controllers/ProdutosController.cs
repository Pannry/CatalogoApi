using ApiCatalogo._6.Context;
using ApiCatalogo._6.DTOs;
using ApiCatalogo._6.Models;
using ApiCatalogo._6.Pagination;
using ApiCatalogo._6.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApiCatalogo._6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        // Sempre que o objeto for mostrado para o cliente, converter para ProdutoDTO
        // Sempre que o objeto for para o banco de dados, converter para Produto

        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPreco()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            return _mapper.Map<List<ProdutoDTO>>(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

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

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
           
            if(produto == null)
                return NotFound("Produto não encontrado");

            return _mapper.Map<ProdutoDTO>(produto);
        }

        [HttpPost]
        public ActionResult Post(ProdutoDTO produtoDto)
        {
            // Atualmente não é mais necessário implementar cádigo abaixo, pois esse trecho
            // já esta implementado em [ApiController] (Desde .net core 2)
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var produto = _mapper.Map<Produto>(produtoDto);

            if (produto == null)
                return BadRequest();

            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            // CreatedAtRouteResult se o produto for cadastrado no banco, retorne os valores
            // cadastrados pela rota onde possui o GET com a propriedade Name = "ObterProduto"
            return new CreatedAtRouteResult("ObterProduto", new {id = produto.ProdutoId}, produtoDTO);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
                return BadRequest();

            // Informa que o objeto produto possui valores modificados e salva as informações

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(produtoDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado");

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }
    }
}
