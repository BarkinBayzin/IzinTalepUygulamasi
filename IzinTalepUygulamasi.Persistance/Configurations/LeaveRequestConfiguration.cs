using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.HasKey(l => l.Id);
        builder.HasOne(l => l.AssignedUser).WithMany(x => x.LeaveRequests).HasForeignKey(l => l.AssignedUserId);
        builder.HasOne(l => l.CreatedBy).WithMany().HasForeignKey(l => l.CreatedById);
        builder.HasOne(l => l.LastModifiedBy).WithMany().HasForeignKey(l => l.LastModifiedById);
        builder.Property(l => l.FormNumber).ValueGeneratedOnAdd();

        // RequestNumber'ı FormNumber üzerinden oluşturduğumuz computed column
        builder.Property(x => x.RequestNumber)
            .HasComputedColumnSql("CONCAT('LRF-', FORMAT(FormNumber, 'D6'))")
            .HasColumnType("nvarchar(max)")
            .IsRequired();
    }
}
