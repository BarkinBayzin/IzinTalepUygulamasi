public class NotificationDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid CumulativeLeaveRequestId { get; set; }
}
