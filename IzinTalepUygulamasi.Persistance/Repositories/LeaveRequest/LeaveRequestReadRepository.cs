public class LeaveRequestReadRepository : ReadRepository<LeaveRequest>, ILeaveRequestReadRepository
{
    public LeaveRequestReadRepository(IzinTalepAPIContext context) : base(context)
    {
    }
}
