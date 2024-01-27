public class LeaveRequestWriteRepository : WriteRepository<LeaveRequest>, ILeaveRequestWriteRepository
{
    public LeaveRequestWriteRepository(IzinTalepAPIContext context) : base(context)
    {
    }
}
