using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApiCatalogoContext context) : base(context)
        {
        }

    }
}

