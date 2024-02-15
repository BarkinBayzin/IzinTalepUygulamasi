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
        var cumulativeLeaveRequest = await _cumulativeLeaveReadRepository.GetCumulativeLeaveRequestAsync(dto.UserId, dto.LeaveType, DateTime.UtcNow.Year);

        if (cumulativeLeaveRequest == null)
        {
            // Kümülatif izin verisi bulunamadıysa, yeni bir kayıt oluşturulur
            cumulativeLeaveRequest = new CumulativeLeaveRequest
            {
                UserId = dto.UserId,
                LeaveType = dto.LeaveType,
                TotalHours = dto.TotalHours,
                Year = (short)DateTime.UtcNow.Year
            };

            return await AddAsync(cumulativeLeaveRequest);
        }
        else
        {
            // Kümülatif izin verisi bulunduysa, mevcut izinlere eklenen saat kadar güncellenir
            cumulativeLeaveRequest.TotalHours += dto.TotalHours;

           return await UpdateAsync(cumulativeLeaveRequest);
        }
    }
    public int CalculateLeaveHours(LeaveType leaveType)
    {
        // İzin tipine göre toplam izin süresini hesapla
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
        return cumulativeLeaveRequest?.TotalHours ?? 0;
    }
}