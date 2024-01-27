using MediatR;
public class CreateLeaveRequestCommand : IRequest<BaseResponse>
{
    public LeaveRequestDTO LeaveRequestDTO { get; set; }
}
