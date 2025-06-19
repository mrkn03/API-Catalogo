using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApiCatalogoContext context) : base(context)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var categoria = GetAll()
                .OrderBy(c => c.CategoriaId)
                .AsQueryable();
            
            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categoria, categoriasParameters.PageNumber, categoriasParameters.PageSize);
            
            return categoriasOrdenadas;
        }
    }
}

