using AutoMapper;
using MediatR;
public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseResponse>
{
    #region DI
    private readonly ILeaveRequestWriteRepository _leaveRequestWriteRepository;
    private readonly IADUserReadRepository _userReadRepository;
    private readonly IMapper _mapper;
    private readonly ICumulativeLeaveWriteRepository _cumulativeLeaveWriteRepository;
    private readonly ICumulativeLeaveReadRepository _cumulativeLeaveReadRepository;
    private readonly INotificationWriteRepository _notificationWriteRepository;
    public CreateLeaveRequestCommandHandler(ILeaveRequestWriteRepository leaveRequestWriteRepository, IMapper mapper, IADUserReadRepository userReadRepository, ICumulativeLeaveWriteRepository cumulativeLeaveWriteRepository, INotificationWriteRepository notificationWriteRepository, ICumulativeLeaveReadRepository cumulativeLeaveReadRepository)
    {
        _leaveRequestWriteRepository = leaveRequestWriteRepository;
        _mapper = mapper;
        _userReadRepository = userReadRepository;
        _cumulativeLeaveWriteRepository = cumulativeLeaveWriteRepository;
        _notificationWriteRepository = notificationWriteRepository;
        _cumulativeLeaveReadRepository = cumulativeLeaveReadRepository;
    }
    #endregion
    public async Task<BaseResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        LeaveRequestDTO? leaveRequestDto = _mapper.Map<LeaveRequestDTO>(request.LeaveRequestDTO);

        ADUser? user = await _userReadRepository.GetByIdAsync(leaveRequestDto.CreatedById.ToString());
        if (user == null)
            return new BaseResponse
            {
                Success = false,
                Message = "Çalışan bulunamadı!"
            };
        leaveRequestDto.UserType = user.UserType;
        leaveRequestDto.LastModifiedById = user.Id;

        // UserType ve LeaveType koşullarına göre Workflow belirlenir
        switch (leaveRequestDto.UserType)
        {
            case UserType.WhiteCollarEmployee:
                leaveRequestDto.WorkflowStatus = Workflow.Pending;
                leaveRequestDto.AssignedUserId = await GetManagerIdByUserId(user.Id);
                break;

            case UserType.BlueCollarEmployee when leaveRequestDto.LeaveType == LeaveType.AnnualLeave:
                leaveRequestDto.WorkflowStatus = Workflow.Pending;
                leaveRequestDto.AssignedUserId = await GetManagerIdByUserId(user.Id);
                break;

            case UserType.BlueCollarEmployee when leaveRequestDto.LeaveType == LeaveType.ExcusedAbsence:
                leaveRequestDto.WorkflowStatus = Workflow.Pending;
                leaveRequestDto.AssignedUserId = await GetManagerIdByUserId(user.Id);
                break;

            case UserType.Manager:
                leaveRequestDto.WorkflowStatus = Workflow.None;
                leaveRequestDto.AssignedUserId = null;
                break;
        }

        // Talebi contexte ekle
        bool resultLeaveRequest = await _leaveRequestWriteRepository.AddAsync(_mapper.Map<LeaveRequest>(leaveRequestDto));
        if (resultLeaveRequest)
        {
            // Kümülatif verileri güncelle
            var cumulative = await _cumulativeLeaveReadRepository.GetCumulativeLeaveRequestAsync(user.Id, leaveRequestDto.LeaveType, DateTime.UtcNow.Year);

            CumulativeLeaveRequestDTO cumulativeDto = _mapper.Map<CumulativeLeaveRequestDTO>(cumulative);
            if (cumulativeDto == null)
            {
                cumulativeDto = new() 
                { 
                    UserId=request.LeaveRequestDTO.CreatedById, 
                    LeaveType=leaveRequestDto.LeaveType           
                };
                
            }
            cumulativeDto.TotalHours = (short)CalculateWorkingHours(leaveRequestDto.StartDate, leaveRequestDto.EndDate);
            bool resultCumulativeLeaveRequest = await _cumulativeLeaveWriteRepository.UpdateCumulativeLeaveRequestAsync(cumulativeDto);
            leaveRequestDto.CumulativeLeaveRequestId = cumulativeDto.Id;
            if (resultCumulativeLeaveRequest)
            {
                await _notificationWriteRepository.CheckAndCreateNotificationsAsync(leaveRequestDto);
                await _notificationWriteRepository.SaveAsync();
                return new BaseResponse
                {
                    Success = resultCumulativeLeaveRequest,
                    Id = resultCumulativeLeaveRequest ? cumulativeDto.Id.ToString() : default,
                    Message = resultCumulativeLeaveRequest ? "Leave Request, Cumulative Leave Request and Notifications are created!" : "Leave Request, Cumulative Leave Request added but Notification can not created!"
                };
            }
            else
            {
                return new BaseResponse
                {
                    Success = resultCumulativeLeaveRequest,
                    Id = resultCumulativeLeaveRequest ? cumulativeDto.Id.ToString() : default,
                    Message = resultCumulativeLeaveRequest ? "Leave Request Added" : "Leave Request Added But Cumulative Leave Request Can Not Created!"
                };
            }
        }
        return new BaseResponse
        {
            Success = resultLeaveRequest,
            Id = resultLeaveRequest ? leaveRequestDto.Id.ToString() : default,
            Message = resultLeaveRequest ? "Leave Request added" : "Failed"
        };
    }
    private async Task<Guid> GetManagerIdByUserId(Guid userId)
    {
        ADUserDTO dto = _mapper.Map<ADUserDTO>(await _userReadRepository.GetByIdAsync(userId.ToString()));
        return dto.ManagerId;
    }

    private double CalculateWorkingHours(DateTime startDate, DateTime endDate)
    {
        double totalWorkingHours = 0;

        // Calculate working hours for the same day
        if (startDate.Date == endDate.Date)
        {
            double hoursOnSameDay = (endDate - startDate).TotalHours;

            // Check if the current day is not Sunday
            if (startDate.DayOfWeek != DayOfWeek.Sunday)
            {
                // Assuming a standard workday is 8 hours
                totalWorkingHours += Math.Min(hoursOnSameDay, 8);
            }
        }
        else
        {
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                // Check if the current day is not Sunday
                if (currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    // Assuming a standard workday is 8 hours
                    totalWorkingHours += 8;
                }

                // Move to the next day
                currentDate = currentDate.AddDays(1);
            }
        }

        return totalWorkingHours;
    }

}