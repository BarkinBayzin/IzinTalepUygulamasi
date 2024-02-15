
public class NotificationWriteRepository : WriteRepository<Notification>, INotificationWriteRepository
{
    private readonly ICumulativeLeaveWriteRepository _cumulativeWriteRepository;

    public NotificationWriteRepository(IzinTalepAPIContext context, ICumulativeLeaveWriteRepository cumulativeWriteRepository) : base(context)
    {
        _cumulativeWriteRepository = cumulativeWriteRepository;
    }

    public async Task CheckAndCreateNotificationsAsync(LeaveRequestDTO leaveRequestDto)
    {
        // Bildirimleri kontrol etme ve oluşturma işlemleri burada yapılır
        // İzin durumlarına göre kontrol ve bildirim oluşturma

        // İzin tipine ve kullanıcıya özel kontrolleri gerçekleştir
        switch (leaveRequestDto.LeaveType)
        {
            case LeaveType.AnnualLeave:
                await CheckAnnualLeaveNotification(leaveRequestDto);
                break;

            case LeaveType.ExcusedAbsence:
                await CheckExcusedAbsenceNotification(leaveRequestDto);
                break;
        }

        // Her izin tipi için müsade edilen gün sayısının %80'i kullanıldığında kullanıcıya bildirim oluştur
        if (leaveRequestDto.WorkflowStatus != Workflow.Exception && leaveRequestDto.WorkflowStatus != Workflow.None)
        {
            var allowedHours = _cumulativeWriteRepository.CalculateLeaveHours(leaveRequestDto.LeaveType);
            var usedHours = await _cumulativeWriteRepository.GetUsedLeaveHoursAsync(leaveRequestDto.CreatedById, leaveRequestDto.LeaveType, DateTime.UtcNow.Year);

            if (usedHours >= allowedHours * 0.8)
            {
                await CreateNotificationForUsedHours(leaveRequestDto.CreatedById, leaveRequestDto.LeaveType, usedHours, allowedHours, leaveRequestDto.CumulativeLeaveRequestId);
            }
        }
    }

    private async Task CheckAnnualLeaveNotification(LeaveRequestDTO leaveRequestDto)
    {
        // AnnualLeave için özel kontrol ve bildirim işlemleri burada yapılır
        var allowedHours = _cumulativeWriteRepository.CalculateLeaveHours(LeaveType.AnnualLeave);
        var usedHours = await _cumulativeWriteRepository.GetUsedLeaveHoursAsync(leaveRequestDto.CreatedById, LeaveType.AnnualLeave, DateTime.UtcNow.Year);

        if (usedHours > allowedHours)
        {
            var excessPercentage = (usedHours - allowedHours) / (float)allowedHours * 100;

            if (excessPercentage > 10)
            {
                await CreateNotificationForExcessLeave(leaveRequestDto.CreatedById, LeaveType.AnnualLeave, usedHours, allowedHours, leaveRequestDto);
            }
        }
    }
    private async Task CheckExcusedAbsenceNotification(LeaveRequestDTO leaveRequestDto)
    {
        // ExcusedAbsence için özel kontrol ve bildirim işlemleri burada yapılır
        var allowedHours = _cumulativeWriteRepository.CalculateLeaveHours(LeaveType.ExcusedAbsence);
        var usedHours = await _cumulativeWriteRepository.GetUsedLeaveHoursAsync(leaveRequestDto.CreatedById, LeaveType.ExcusedAbsence, DateTime.UtcNow.Year);

        if (usedHours > allowedHours)
        {
            var excessPercentage = (usedHours - allowedHours) / (float)allowedHours * 100;

            if (excessPercentage > 20)
            {
                await CreateNotificationForExcessLeave(leaveRequestDto.CreatedById, LeaveType.ExcusedAbsence, usedHours, allowedHours, leaveRequestDto);
            }
        }
    }

    private async Task CreateNotificationForExcessLeave(Guid userId, LeaveType leaveType, int usedHours, int allowedHours, LeaveRequestDTO leaveRequestDto)
    {
        _cumulativeWriteRepository.ClearContextAsync();
        // İzin süresinin aşıldığı durumda bildirim oluştur
        var notification = new Notification
        {
            UserId = userId,
            Message = NotificationMessage(leaveType, usedHours, allowedHours),
            CreateDate = DateTime.UtcNow,
            CumulativeLeaveRequestId = leaveRequestDto.CumulativeLeaveRequestId
        };

        // Bildirimi ekle
        await AddAsync(notification);
        // İzin talebi Workflow bilgisi her durumda Exception olacak ve kullanıcı ile varsa Manager’a bildirim yapılacaktır.
        leaveRequestDto.WorkflowStatus = Workflow.Exception;

        if (leaveRequestDto.AssignedUserId.HasValue)
        {
            // Manager'a bildirim yap
            var managerNotification = new Notification
            {
                UserId = leaveRequestDto.AssignedUserId.Value,
                Message = NotificationMessage(leaveType, usedHours, allowedHours),
                CreateDate = DateTime.UtcNow,
                CumulativeLeaveRequestId = leaveRequestDto.CumulativeLeaveRequestId
            };

            // Manager bildirimini kaydet
            await AddAsync(managerNotification);
        }
        // Bildirimi kaydet
        await SaveAsync();
    }


    private async Task CreateNotificationForUsedHours(Guid userId, LeaveType leaveType, int usedHours, int allowedHours, Guid? cumulativeRequestId)
    {
        // Kullanıcıya kullanılan izin süresinin %80'i aşıldığında bildirim oluştur

        string message = NotificationMessage(leaveType, usedHours, allowedHours);

        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            CreateDate = DateTime.UtcNow,
            CumulativeLeaveRequestId = cumulativeRequestId
        };

        // Bildirimi kaydet
        await AddAsync(notification);
        await SaveAsync();
    }

    private string NotificationMessage(LeaveType leaveType, int usedHours, int allowedHours)
    {
        // Calculate remaining and exceeded hours
        int remainingHours = allowedHours - usedHours;
        int exceededHours = usedHours - allowedHours;

        // Determine the notification message based on the situation
        string message = "";
        if (remainingHours <= 0)
        {
            // Exceeded hours case
            message = $"Aşılan {leaveType} {exceededHours} saat";
        }
        else if (exceededHours > 0)
        {
            // Both remaining and exceeded hours case
            message = $"Kalan {leaveType} {remainingHours} saat, Aşılan {leaveType} {exceededHours} saat";
        }
        else
        {
            // Remaining hours case
            message = $"Kalan {leaveType} {remainingHours} saat";
        }

        return message;
    }
}
