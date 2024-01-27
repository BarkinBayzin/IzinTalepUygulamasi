public interface ICumulativeLeaveReadRepository : IReadRepository<CumulativeLeaveRequest>
{
    Task<CumulativeLeaveRequest> GetCumulativeLeaveRequestAsync(Guid userId, LeaveType leaveType, int year);
}

