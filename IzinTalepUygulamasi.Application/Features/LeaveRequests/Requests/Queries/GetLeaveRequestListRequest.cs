using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
public class GetLeaveRequestListRequest : IRequest<BasePaginationResponse<List<GetLeaveRequestListRequest>>>
{
    [Display(Name = "ReqFormNumber")]
    public string? RequestNumber { get; set; }
    public LeaveType LeaveType { get; set; }
    [Display(Name = "CreateDate")]
    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public short TotalHour => CalculateTotalHour();
    private short CalculateTotalHour()
    {
        TimeSpan duration = EndDate - StartDate;
        return (short)duration.TotalHours;
    }
    public Workflow WorkflowStatus { get; set; }
    public string FullName { get; set; }
    [JsonIgnore]
    public Guid CreatedById { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}