using FluentValidation;
public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestValidator()
    {
        RuleFor(p => 
        p.LeaveRequestDTO.CreatedById)
            .NotEmpty()
            .NotNull()
            .WithMessage("Çalışan bilgisi boş bırakılamaz!");
        RuleFor(p => p.LeaveRequestDTO.StartDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("İzin başlangıç tarihi boş bırakılamaz!");
        RuleFor(p => p.LeaveRequestDTO.EndDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("İzin bitiş tarihi boş bırakılamaz!");
        RuleFor(p => p.LeaveRequestDTO.LeaveType).NotEmpty().NotNull().WithMessage("İzin tipi boş bırakılamaz!");
    }
}