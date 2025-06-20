using APICatalogo.Context;
using APICatalogo.Repositories.Interfaces;
using APICatalogo.Repository;

namespace APICatalogo.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutoRepository? produtoRepository;
        private ICategoriaRepository? categoriaRepository;

        public ApiCatalogoContext context;

        public UnitOfWork(ApiCatalogoContext context)
        {
            this.context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
               return produtoRepository ??= new ProdutoRepository(context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return categoriaRepository ??= new CategoriaRepository(context);
            }
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();            
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
