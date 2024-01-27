public class LeaveRequestDTO
{
    public LeaveRequestDTO()
    {

        CreatedAt = DateTime.UtcNow;
        Id = Guid.NewGuid();
        LastModifiedAt = CreatedAt;
    }
    public Guid Id { get; set; }
    public LeaveType LeaveType { get; set; }
    public string Reason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Workflow WorkflowStatus { get; set; }
    public UserType? UserType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }

    public Guid? AssignedUserId { get; set; }
    public Guid CreatedById { get; set; }
    public Guid? LastModifiedById { get; set; }

    public Guid? CumulativeLeaveRequestId { get; set; }
}
