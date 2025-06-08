﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApiCatalogoContext context) : base(context)
        {

        }

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {

            var produtos = GetAll()
                .OrderBy(p => p.ProdutoId)
                .AsQueryable();

            var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosOrdenados;
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(p => p.CategoriaId == id);
        }
    }
}
