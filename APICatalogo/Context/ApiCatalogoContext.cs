using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context;

public class ApiCatalogoContext : DbContext
{
    public ApiCatalogoContext(DbContextOptions<ApiCatalogoContext> options) : base(options)
    {
    }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
   
    

}
