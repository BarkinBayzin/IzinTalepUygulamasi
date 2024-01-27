using AutoMapper;
using MediatR;

public class GetLeaveRequestListHandler : IRequestHandler<GetLeaveRequestListRequest, BasePaginationResponse<List<GetLeaveRequestListRequest>>>
{
    private readonly ILeaveRequestReadRepository _leaveRequestReadRepository;
    private readonly IMapper _mapper;

    public GetLeaveRequestListHandler(ILeaveRequestReadRepository leaveRequestReadRepository, IMapper mapper)
    {
        _leaveRequestReadRepository = leaveRequestReadRepository;
        _mapper = mapper;
    }

    public async Task<BasePaginationResponse<List<GetLeaveRequestListRequest>>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
    {
        int totalCount = _leaveRequestReadRepository.GetAll(false).Count(); // Toplam kayıt sayısını al
        int skip = (request.PageNumber - 1) * request.PageSize;

        List<GetLeaveRequestListRequest> leaveRequests = _mapper.Map<List<GetLeaveRequestListRequest>>(
            _leaveRequestReadRepository.GetAll(false, x => x.CreatedBy)
                .Skip(skip)
                .Take(request.PageSize)
                .ToList());

        var response = new BasePaginationResponse<List<GetLeaveRequestListRequest>>(
            data: leaveRequests,
            success: true,
            message: "Leave requests retrieved successfully",
            currentPageIndex: request.PageNumber,
            totalCount: totalCount,
            pageSize: request.PageSize);

        return response;
    }
}