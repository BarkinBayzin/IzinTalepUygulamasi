using AutoMapper;

public class CumulativeLeaveRequestProfile : Profile
{
    public CumulativeLeaveRequestProfile()
    {
        CreateMap<CumulativeLeaveRequest, CumulativeLeaveRequestDTO>().ReverseMap();
    }
}