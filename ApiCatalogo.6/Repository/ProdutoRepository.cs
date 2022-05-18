﻿using ApiCatalogo._6.Context;
using ApiCatalogo._6.Models;
using ApiCatalogo._6.Pagination;

namespace ApiCatalogo._6.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        // Passando o DbContext para a classe base, Repository<Produto>
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            //return Get()
            //    .OrderBy(on => on.Nome)
            //    .Skip((produtosParameter.PageNumber - 1) * produtosParameter.PageSize)
            //    .Take(produtosParameter.PageSize)
            //    .ToList();

            return PagedList<Produto>.ToPagedList(Get()
                .OrderBy(on => on.CategoriaId), produtosParameters.PageNumber, produtosParameters.PageSize);
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}