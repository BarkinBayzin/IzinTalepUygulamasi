public class CumulativeLeaveRequestDTO
{
    public Guid? Id { get; set; }
    public LeaveType LeaveType { get; set; }
    public Guid UserId { get; set; }
    public short TotalHour { get; set; }
    public short Year { get; set; }
}
