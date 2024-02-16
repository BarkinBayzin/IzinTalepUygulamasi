public class ADUser:BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public UserType UserType { get; private set; }
    public Guid? ManagerId { get; private set; }

    //Navigation Properties

    public ICollection<Notification> Notifications { get; private set; }
    public ICollection<CumulativeLeaveRequest> CumulativeLeaveRequests { get; private set; }
    public ICollection<LeaveRequest> LeaveRequests { get; private set; }
    public virtual ADUser? Manager { get; private set; }
    //for seed data
    public ADUser(Guid id, string firstName, string lastName, string email, UserType userType, Guid? managerId)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserType = userType;
        ManagerId = managerId;
    }
    public ADUser() { }    
}

