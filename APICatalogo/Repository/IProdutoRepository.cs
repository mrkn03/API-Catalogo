using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> GetProdutos();
        Produto GetProduto(int id);
        Produto Add(Produto produto);
        Produto Update(Produto produto);
        Produto Delete(int id);
    }
}
