using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

public class ADUserConfiguration : IEntityTypeConfiguration<ADUser>
{
    public void Configure(EntityTypeBuilder<ADUser> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasMany(u => u.Notifications).WithOne(n => n.User).HasForeignKey(n => n.UserId);
        builder.HasMany(u => u.CumulativeLeaveRequests).WithOne(c => c.User).HasForeignKey(c => c.UserId);
        builder.HasOne(u => u.Manager).WithMany().HasForeignKey(u => u.ManagerId);
    }
}
