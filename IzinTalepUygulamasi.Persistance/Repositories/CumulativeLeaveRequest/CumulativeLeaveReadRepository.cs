using Microsoft.EntityFrameworkCore;

public class CumulativeLeaveReadRepository : ReadRepository<CumulativeLeaveRequest>, ICumulativeLeaveReadRepository
{
    private readonly IzinTalepAPIContext _context;
    public CumulativeLeaveReadRepository(IzinTalepAPIContext context) : base(context)
    {
        _context = context;
    }
    public async Task<CumulativeLeaveRequest> GetCumulativeLeaveRequestAsync(Guid userId, LeaveType leaveType, int year) =>
         await _context.CumulativeLeaveRequests
            .FirstOrDefaultAsync(clr => clr.UserId == userId && clr.LeaveType == leaveType && clr.Year == year);
}
