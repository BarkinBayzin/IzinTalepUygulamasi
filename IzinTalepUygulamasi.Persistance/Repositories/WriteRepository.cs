﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
{
    private readonly IzinTalepAPIContext _context;
    public WriteRepository(IzinTalepAPIContext context) { _context = context; }
    public DbSet<T> Table => _context.Set<T>();
    public async Task<bool> AddAsync(T entity)
    {
        EntityEntry<T> entityEntry = await Table.AddAsync(entity);
        return await SaveAsync();
    }
    public async Task<bool> UpdateAsync(T entity)
    {
        EntityEntry<T> entityEntry = Table.Update(entity);
        return await SaveAsync();
    }
    public bool Destroy(T entity)
    {
        EntityEntry<T> entityEntry = Table.Remove(entity);
        return entityEntry.State == EntityState.Deleted;
    }

    public async Task<bool> SaveAsync()
    {
        if (await _context.SaveChangesAsync() == 0)
            return false;
        else return true;
    }
}