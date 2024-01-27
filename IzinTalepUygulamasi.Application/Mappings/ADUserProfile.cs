using AutoMapper;

public class ADUserProfile : Profile
{
    public ADUserProfile()
    {
        CreateMap<ADUser, ADUserDTO>().ReverseMap();
    }
}

