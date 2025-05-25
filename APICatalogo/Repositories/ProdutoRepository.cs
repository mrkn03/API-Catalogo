using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApiCatalogoContext context) : base(context)
        {

        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(p => p.CategoriaId == id);
        }
    }
}
