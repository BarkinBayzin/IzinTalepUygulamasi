using System.Linq.Expressions;
public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll(bool tracking = true, params Expression<Func<T, object>>[] includes);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = true, params Expression<Func<T, object>>[] includes);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool tracking = true, params Expression<Func<T, object>>[] includes);
    Task<T> GetByIdAsync(string id, bool tracking = false, params Expression<Func<T, object>>[] includes);
}
