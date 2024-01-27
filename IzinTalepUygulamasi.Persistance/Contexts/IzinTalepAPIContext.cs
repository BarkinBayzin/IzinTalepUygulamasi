using Microsoft.EntityFrameworkCore;
using System.Reflection;
public class IzinTalepAPIContext : DbContext
{
    public IzinTalepAPIContext(DbContextOptions<IzinTalepAPIContext> options) : base(options) { }
    public DbSet<ADUser> ADUsers => Set<ADUser>();
    public DbSet<CumulativeLeaveRequest> CumulativeLeaveRequests => Set<CumulativeLeaveRequest>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data konfigürasyonları...
        modelBuilder.Entity<ADUser>().Seed();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var changedData = ChangeTracker
             .Entries<LeaveRequest>();

        if(changedData != null)
        foreach (var data in changedData)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedAt = DateTime.UtcNow,
                EntityState.Modified => data.Entity.LastModifiedAt = DateTime.UtcNow,
                _ => DateTime.UtcNow
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
