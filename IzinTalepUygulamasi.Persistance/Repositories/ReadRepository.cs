using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    private readonly IzinTalepAPIContext _context;

    public ReadRepository(IzinTalepAPIContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>();

    public IQueryable<T> GetAll(bool tracking = true, params Expression<Func<T, object>>[] includes)
    {
        var query = Table.AsQueryable();

        if (!tracking)
            query = query.AsNoTracking();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return query;
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = true, params Expression<Func<T, object>>[] includes)
    {
        var query = Table.Where(expression);

        if (!tracking)
            query = query.AsNoTracking();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return query;
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool tracking = true, params Expression<Func<T, object>>[] includes)
    {
        var query = Table.AsQueryable();

        if (!tracking)
            query = query.AsNoTracking();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task<T> GetByIdAsync(string id, bool tracking = true, params Expression<Func<T, object>>[] includes)
    {
        var query = Table.AsQueryable();

        if (!tracking)
            query = query.AsNoTracking();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
    }
}

