public class CumulativeLeaveRequest:BaseEntity
{
    public LeaveType LeaveType { get; set; }
    public Guid UserId { get; set; }
    public short TotalHours { get; set; }
    public short Year { get; set; }

    // Navigation Properties

    public ADUser User { get; set; }
    public ICollection<Notification> Notifications { get; set; }
}
