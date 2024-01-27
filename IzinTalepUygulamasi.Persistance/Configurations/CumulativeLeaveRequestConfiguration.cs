using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CumulativeLeaveRequestConfiguration : IEntityTypeConfiguration<CumulativeLeaveRequest>
{
    public void Configure(EntityTypeBuilder<CumulativeLeaveRequest> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasMany(c => c.Notifications).WithOne(n => n.CumulativeLeaveRequest).HasForeignKey(n => n.CumulativeLeaveRequestId);
        builder.HasOne(c => c.User).WithMany(u => u.CumulativeLeaveRequests).HasForeignKey(c => c.UserId);
    }
}
