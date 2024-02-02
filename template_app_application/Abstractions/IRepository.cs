using System.Linq.Expressions;

namespace template_app_application.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        public Task<TEntity> GetByIdAsync(object id);

        public void Add(TEntity entity);

        public void Delete(object id);

        public void Delete(TEntity entityToDelete);

        public void Update(TEntity entityToUpdate);
    }
}