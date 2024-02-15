using AutoMapper;
using MediatR;

public class GetLeaveRequestListHandler : IRequestHandler<GetLeaveRequestListRequest, BasePaginationResponse<List<LeaveRequestResponseModel>>>
{
    private readonly ILeaveRequestReadRepository _leaveRequestReadRepository;
    private readonly IMapper _mapper;

    public GetLeaveRequestListHandler(ILeaveRequestReadRepository leaveRequestReadRepository, IMapper mapper)
    {
        _leaveRequestReadRepository = leaveRequestReadRepository;
        _mapper = mapper;
    }

    public async Task<BasePaginationResponse<List<LeaveRequestResponseModel>>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
    {
        int totalCount = _leaveRequestReadRepository.GetAll(false).Count(); // Toplam kayıt sayısını al
        int skip = (request.PageNumber - 1) * request.PageSize;

        List<GetLeaveRequestListRequest> leaveRequests = _mapper.Map<List<GetLeaveRequestListRequest>>(
            _leaveRequestReadRepository.GetAll(false, x => x.CreatedBy)
                .Skip(skip)
                .Take(request.PageSize)
                .ToList());

        List<LeaveRequestResponseModel> responseModel = new List<LeaveRequestResponseModel>();
        foreach (GetLeaveRequestListRequest lr in leaveRequests)
        {
            responseModel.Add(new LeaveRequestResponseModel
            {
                ReqFormNumber = lr.RequestNumber,
                FullName = lr.FullName,
                LeaveType = lr.LeaveType.ToString(),
                CreateDate = lr.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                StartDate = lr.StartDate.ToString("dd.MM.yyyy"),
                EndDate = lr.EndDate.ToString("dd.MM.yyyy"),
                TotalHour = lr.TotalHour,
                Workflow = lr.WorkflowStatus.ToString(),
            });
        }

        var response = new BasePaginationResponse<List<LeaveRequestResponseModel>>(
            data: responseModel,
            success: true,
            message: "Leave requests retrieved successfully",
            currentPageIndex: request.PageNumber,
            totalCount: totalCount,
            pageSize: request.PageSize);

        return response;
    }


}