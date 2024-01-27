using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.HasOne(n => n.User).WithMany(u => u.Notifications).HasForeignKey(n => n.UserId);
        builder.HasOne(n => n.CumulativeLeaveRequest).WithMany(c => c.Notifications).HasForeignKey(n => n.CumulativeLeaveRequestId).OnDelete(DeleteBehavior.Restrict);
    }
}
