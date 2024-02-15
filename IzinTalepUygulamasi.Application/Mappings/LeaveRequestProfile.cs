﻿using AutoMapper;

public class LeaveRequestProfile : Profile
    {
    public LeaveRequestProfile()
    {
        CreateMap<LeaveRequest, LeaveRequestDTO>().ReverseMap();
        CreateMap<LeaveRequest, GetLeaveRequestListRequest>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("dd.MM.yyyy HH:mm")))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToShortDateString()))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToShortDateString()))
            .ForMember(dest => dest.WorkflowStatus, opt => opt.MapFrom(src => (int)src.WorkflowStatus))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}"))
            .ReverseMap();
    }
}
