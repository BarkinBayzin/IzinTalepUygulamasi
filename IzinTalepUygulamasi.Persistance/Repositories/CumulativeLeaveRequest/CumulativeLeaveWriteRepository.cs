public class CumulativeLeaveWriteRepository : WriteRepository<CumulativeLeaveRequest>, ICumulativeLeaveWriteRepository 
{
    private readonly ICumulativeLeaveReadRepository _cumulativeLeaveReadRepository;
    public CumulativeLeaveWriteRepository(IzinTalepAPIContext context, ICumulativeLeaveReadRepository cumulativeLeaveReadRepository) : base(context)
    {
        _cumulativeLeaveReadRepository = cumulativeLeaveReadRepository;
    }

    public async Task<bool> UpdateCumulativeLeaveRequestAsync(CumulativeLeaveRequestDTO dto)
    {
        // Kümülatif izin verilerini güncelleme işlemleri burada yapılır
        // LeaveType ve UserId'ye göre güncelleme
        var cumulativeLeaveRequest = await _cumulativeLeaveReadRepository.GetCumulativeLeaveRequestAsync(dto.UserId, dto.LeaveType, DateTime.Now.Year);

        if (cumulativeLeaveRequest == null) // Kümülatif izin verisi bulunamadıysa, yeni bir kayıt oluşturulur
            return await AddAsync(new CumulativeLeaveRequest(dto.LeaveType, dto.UserId, dto.TotalHour, (short)DateTime.Now.Year));
        else
        {
            // Kümülatif izin verisi bulunduysa, mevcut izinlere eklenen saat kadar güncellenir
            cumulativeLeaveRequest.TotalHoursUpdater(dto.TotalHour);
           return Update(cumulativeLeaveRequest);
        }
    }
    /// <summary>
    /// İzin tipine göre, toplam izin verilen saat bilgisini hesaplanır.
    /// </summary>
    /// <param name="leaveType">İzin tipi</param>
    /// <returns>Yıllık toplam izin verilen süre</returns>
    public int CalculateLeaveHours(LeaveType leaveType)
    {
        switch (leaveType)
        {
            case LeaveType.AnnualLeave:
                return 14 * 8; // Senelik izin süresi: 14 gün * 8 saat
            case LeaveType.ExcusedAbsence:
                return 5 * 8; // Mazeret izin süresi: 5 gün * 8 saat
            default:
                return 0;
        }
    }

    public async Task<int> GetUsedLeaveHoursAsync(Guid userId, LeaveType leaveType, int year)
    {
        var cumulativeLeaveRequest = await _cumulativeLeaveReadRepository.GetCumulativeLeaveRequestAsync(userId, leaveType, year);
        return cumulativeLeaveRequest?.TotalHour ?? 0;
    }
}