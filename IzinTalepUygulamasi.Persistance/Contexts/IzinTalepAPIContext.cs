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
                EntityState.Added => data.Entity.CreatedAt = DateTime.Now,
                EntityState.Modified => data.Entity.LastModifiedAt = DateTime.Now,
                _ => DateTime.Now
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
