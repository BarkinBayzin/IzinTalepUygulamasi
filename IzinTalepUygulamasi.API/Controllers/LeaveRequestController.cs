using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IzinTalepUygulamasi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveRequestCommand request)
        {
            try
            {
                await _mediator.Send(request);
                return Created("", request);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<LeaveRequestController>/{pageNumber}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1)
        {
            try
            {
                return Ok(await _mediator.Send(new GetLeaveRequestListRequest { PageNumber = pageNumber }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
