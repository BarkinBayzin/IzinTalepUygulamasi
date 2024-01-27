using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IzinTalepAPIContext>
{
    public IzinTalepAPIContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<IzinTalepAPIContext> dbContextOptionsBuilder = new();
        dbContextOptionsBuilder.UseSqlServer(Configuration.ConnectionString);
        return new(dbContextOptionsBuilder.Options);
    }
}
