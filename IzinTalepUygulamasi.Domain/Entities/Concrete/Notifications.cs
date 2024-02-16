public class Notification:BaseEntity
{
    public Notification(Guid userId, string message, DateTime createDate, Guid? cumulativeLeaveRequestId)
    {
        UserId = userId;
        Message = message;
        CreateDate = createDate;
        CumulativeLeaveRequestId = cumulativeLeaveRequestId;
    }

    public Guid UserId { get; private set; }
    public string Message { get; private set; }
    public DateTime CreateDate { get; private set; }
    public Guid? CumulativeLeaveRequestId { get; private set; }

    //Navigation Properties
    public ADUser User { get; private set; }
    public CumulativeLeaveRequest CumulativeLeaveRequest { get; private set; }
}

