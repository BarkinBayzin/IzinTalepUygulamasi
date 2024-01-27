
public class NotificationReadRepository : ReadRepository<Notification>, INotificationReadRepository
{
    public NotificationReadRepository(IzinTalepAPIContext context) : base(context)
    {
    }
}

