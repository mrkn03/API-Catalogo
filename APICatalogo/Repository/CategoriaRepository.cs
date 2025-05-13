using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApiCatalogoContext context;

        public CategoriaRepository(ApiCatalogoContext context)
        {
            this.context = context;
        }

        public Categoria Add(Categoria categoria)
        {
            if(categoria is null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }

            context.Categorias.Add(categoria);
            context.SaveChanges();

            return categoria;
        }

        public Categoria Update(Categoria categoria)
        {
            if (categoria is null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }

            context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return categoria;
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            return context.Categorias.ToList();
        }

        public Categoria GetCategoria(int id)
        {
            var categoria = context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria == null)
            {
                throw new Exception("Categoria nao encontrada");
            }

            return categoria;
        }

        public Categoria Delete(int id)
        {
            var categoria = context.Categorias.Find(id);

            if (categoria == null)
            {
                throw new Exception("Categoria nao encontrada");
            }

            context.Categorias.Remove(categoria);
            context.SaveChanges();

            return categoria;
        }
    }
}
