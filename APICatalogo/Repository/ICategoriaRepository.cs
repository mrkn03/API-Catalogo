using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetCategorias();
        Categoria GetCategoria(int id);
        Categoria Add(Categoria categoria);
        Categoria Update(Categoria categoria);
        Categoria Delete(int id);
         
    }
}
