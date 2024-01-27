
public class NotificationWriteRepository : WriteRepository<Notification>, INotificationWriteRepository
{
    private readonly ICumulativeLeaveWriteRepository  _cumulativeWriteRepository;

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
        // İzin süresinin aşıldığı durumda bildirim oluştur
        var notification = new Notification
        {
            UserId = userId,
            Message = $"Yıllık izin süresi aşıldı. Kullanılan saat: {usedHours}, İzin limiti: {allowedHours}",
            CreateDate = DateTime.UtcNow,
           CumulativeLeaveRequestId = leaveRequestDto.CumulativeLeaveRequestId
        };

        // Bildirimi kaydet
        await AddAsync(notification);

        // İzin talebi Workflow bilgisi her durumda Exception olacak ve kullanıcı ile varsa Manager’a bildirim yapılacaktır.
        leaveRequestDto.WorkflowStatus = Workflow.Exception;

        if (leaveRequestDto.AssignedUserId.HasValue)
        {
            // Manager'a bildirim yap
            var managerNotification = new Notification
            {
                UserId = leaveRequestDto.AssignedUserId.Value,
                Message = $"Çalışanınızın izin talebi bir istisna durumu oluşturdu. Detaylar için kontrol ediniz.",
                CreateDate = DateTime.UtcNow,
           CumulativeLeaveRequestId = leaveRequestDto.CumulativeLeaveRequestId
            };

            // Manager bildirimini kaydet
            await AddAsync(managerNotification);
        }
    }


    private async Task CreateNotificationForUsedHours(Guid userId, LeaveType leaveType, int usedHours, int allowedHours, Guid? cumulativeRequestId)
    {
        // Kullanıcıya kullanılan izin süresinin %80'i aşıldığında bildirim oluştur
        var notification = new Notification
        {
            UserId = userId,
            Message = $"Kullanılan izin süresi, izin limitinin %80'ini aştı. Kullanılan saat: {usedHours}, İzin limiti: {allowedHours}",
            CreateDate = DateTime.UtcNow,
           CumulativeLeaveRequestId = cumulativeRequestId
        };

        // Bildirimi kaydet
        await AddAsync(notification);
    }
}
