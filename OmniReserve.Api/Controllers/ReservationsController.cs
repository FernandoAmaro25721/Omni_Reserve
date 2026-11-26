using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Application.Reservations.Queries.GetAllReservations;
using OmniReserve.Domain.Constants;

namespace OmniReserve.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ISender _mediator;

    public ReservationsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllReservations(CancellationToken cancellationToken)
    {
        var reservations = await _mediator.Send(new GetAllReservationsQuery(), cancellationToken);
        return Ok(reservations);
    }
}