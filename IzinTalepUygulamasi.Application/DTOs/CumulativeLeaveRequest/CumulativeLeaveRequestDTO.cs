public class CumulativeLeaveRequestDTO
{
    public Guid? Id { get; set; }
    public LeaveType LeaveType { get; set; }
    public Guid UserId { get; set; }
    public short TotalHours { get; set; }
    public short Year { get; set; }
}
