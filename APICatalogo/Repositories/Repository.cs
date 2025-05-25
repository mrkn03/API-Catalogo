using System.Linq.Expressions;
using APICatalogo.Context;
using APICatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApiCatalogoContext context;

        public Repository(ApiCatalogoContext context)
        {
            this.context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().AsNoTracking().ToList();
        }

        public T? Get(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().FirstOrDefault(predicate);
        }

        public T Create(T entity)
        {
            context.Set<T>().Add(entity);

            return entity;
        }

        public T Delete(T entity)
        {

            context.Set<T>().Remove(entity);

            return entity;

        }

        public T Update(T entity)
        {
            context.Set<T>().Update(entity);

            return entity;
        }
    }
}
