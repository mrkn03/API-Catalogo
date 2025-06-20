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

        public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            var categoria = categorias
                .OrderBy(c => c.CategoriaId)
                .AsQueryable();

            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categoria, categoriasParameters.PageNumber, categoriasParameters.PageSize);
            
            return categoriasOrdenadas;
        }

        public async Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams)
        {
            var categorias = await GetAllAsync();

            if(!string.IsNullOrEmpty(categoriasParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.ToLower().Contains(categoriasParams.Nome.ToLower()));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasParams.PageNumber, categoriasParams.PageSize);

            return categoriasFiltradas;
        }
    }
}

