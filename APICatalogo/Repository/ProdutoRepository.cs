using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApiCatalogoContext context;
        public ProdutoRepository(ApiCatalogoContext context)
        {
            this.context = context;
        }

        public Produto Add(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            context.Produtos.Add(produto);
            context.SaveChanges();

            return produto;
        }

        public Produto Delete(int id)
        {
            var produto = context.Produtos.Find(id);

            if (produto == null)
            {
                throw new Exception("Produto não encontrado");
            }

            context.Produtos.Remove(produto);
            context.SaveChanges();

            return produto;
        }

        public Produto GetProduto(int id)
        {
            var produto = context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
            {
                throw new Exception("Produto não encontrado");
            }

            return produto;
        }

        public IEnumerable<Produto> GetProdutos()
        {
            var produtos = context.Produtos.ToList();

            if (produtos == null || !produtos.Any())
            {
                throw new Exception("Nenhum produto encontrado");
            }

            return produtos;
        }

        public Produto Update(Produto produto)
        {

            context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return produto;
        }
    }
}
