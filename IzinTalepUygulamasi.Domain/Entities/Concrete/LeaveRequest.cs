using System.ComponentModel.DataAnnotations.Schema;

public class LeaveRequest:BaseEntity
{
    public LeaveRequest(LeaveType leaveType, DateTime startDate, DateTime endDate, Guid createdById, string reason)
    {
        LeaveType = leaveType;
        StartDate = startDate;
        EndDate = endDate;
        CreatedById = createdById;
        Reason = reason;
        LastModifiedAt = CreatedAt;
    }
    public int FormNumber { get; private set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string RequestNumber { get; private set; }
    public LeaveType LeaveType { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public Workflow WorkflowStatus { get; private set; }
    public string Reason { get; private set; }

    // AssignedUser ilişkisi
    public Guid? AssignedUserId { get; private set; }
    public virtual ADUser? AssignedUser { get; private set; }

    public DateTime CreatedAt { get; set; }

    // CreatedBy ilişkisi
    public Guid CreatedById { get; private set; }
    public virtual ADUser CreatedBy { get; private set; }

    public DateTime LastModifiedAt { get; set; }

    // LastModifiedBy ilişkisi
    public Guid? LastModifiedById { get; set; }
    public virtual ADUser? LastModifiedBy { get; set; }
}


