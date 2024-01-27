public class ADUser:BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public UserType UserType { get; set; }
    public Guid? ManagerId { get; set; }

    //Navigation Properties

    public ICollection<Notification> Notifications { get; set; }
    public ICollection<CumulativeLeaveRequest> CumulativeLeaveRequests { get; set; }
    public ICollection<LeaveRequest> LeaveRequests { get; set; }

    //Self-join with Lazy Loading
    public virtual ADUser? Manager { get; set; }

}

