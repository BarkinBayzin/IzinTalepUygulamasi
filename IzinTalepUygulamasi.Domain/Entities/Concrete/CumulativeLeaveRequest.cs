public class CumulativeLeaveRequest:BaseEntity
{
    public CumulativeLeaveRequest(LeaveType leaveType, Guid userId, short totalHour, short year)
    {
        LeaveType = leaveType;
        UserId = userId;
        TotalHour = totalHour;
        Year = year;
    }

    public CumulativeLeaveRequest(LeaveType leaveType, Guid userId, short totalHour, short year, ADUser user, ICollection<Notification> notifications)
    {
        LeaveType = leaveType;
        UserId = userId;
        TotalHour = totalHour;
        Year = year;
        User = user;
        Notifications = notifications;
    }

    public LeaveType LeaveType { get; private set; }
    public Guid UserId { get; private set; }
    public short TotalHour { get; private set; }
    public short Year { get; private set; }

    public void TotalHoursUpdater(short totalHours) => TotalHour += totalHours;

    // Navigation Properties

    public ADUser User { get; private set; }
    public ICollection<Notification> Notifications { get; private set; }
}
