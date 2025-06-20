using APICatalogo.Context;
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

        public async Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {

            var produtos = await GetAllAsync();

            var produtosOrdenados = produtos
                .OrderBy(p => p.ProdutoId)
                .AsQueryable();

            var resultado = PagedList<Produto>.ToPagedList(produtosOrdenados, produtosParameters.PageNumber, produtosParameters.PageSize);

            return resultado;
        }

        public Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {

            var produtos = await GetAllAsync();


            return produtos.Where(p => p.CategoriaId == id);
        }
    }
}
