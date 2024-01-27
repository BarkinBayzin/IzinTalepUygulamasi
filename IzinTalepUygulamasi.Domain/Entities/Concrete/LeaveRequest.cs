using System.ComponentModel.DataAnnotations.Schema;

public class LeaveRequest:BaseEntity
{
    public int FormNumber { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string RequestNumber { get; set; }
    public LeaveType LeaveType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Workflow WorkflowStatus { get; set; }
    public string Reason { get; set; }

    // AssignedUser ilişkisi
    public Guid? AssignedUserId { get; set; }
    public virtual ADUser? AssignedUser { get; set; }

    public DateTime CreatedAt { get; set; }

    // CreatedBy ilişkisi
    public Guid CreatedById { get; set; }
    public virtual ADUser? CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    // LastModifiedBy ilişkisi
    public Guid? LastModifiedById { get; set; }
    public virtual ADUser? LastModifiedBy { get; set; }
}


