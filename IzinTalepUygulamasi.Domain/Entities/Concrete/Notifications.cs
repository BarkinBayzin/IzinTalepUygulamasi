public class Notification:BaseEntity
{
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid? CumulativeLeaveRequestId { get; set; }

    //Navigation Properties
    public ADUser User { get; set; }
    public CumulativeLeaveRequest CumulativeLeaveRequest { get; set; }
}

