public interface INotificationWriteRepository : IWriteRepository<Notification>
{
    Task CheckAndCreateNotificationsAsync(LeaveRequestDTO leaveRequestDto);
}
