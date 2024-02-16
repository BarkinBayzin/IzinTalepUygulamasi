using Microsoft.EntityFrameworkCore;
public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
{
    Task<bool> AddAsync(T entity);
    bool Update(T entity);
    bool Destroy(T entity);
    Task<bool> SaveAsync();
    void ClearContextAsync();
}
