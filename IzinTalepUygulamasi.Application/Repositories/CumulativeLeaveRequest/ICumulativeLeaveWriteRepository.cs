public interface ICumulativeLeaveWriteRepository: IWriteRepository<CumulativeLeaveRequest>
{
    Task<bool> UpdateCumulativeLeaveRequestAsync(CumulativeLeaveRequestDTO dto);
    public int CalculateLeaveHours(LeaveType leaveType);
    Task<int> GetUsedLeaveHoursAsync(Guid userId, LeaveType leaveType, int year);
}
