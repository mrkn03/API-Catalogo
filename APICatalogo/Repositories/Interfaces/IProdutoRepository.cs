using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters); 
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
