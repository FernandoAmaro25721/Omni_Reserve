using MediatR;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Reservations.Queries.GetReservationDetails;

public class GetReservationDetailsQueryHandler : IRequestHandler<GetReservationDetailsQuery, ReservationDetailsDto>
{
    private readonly IReservationRepository _reservationRepository;

    public GetReservationDetailsQueryHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<ReservationDetailsDto> Handle(GetReservationDetailsQuery request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId);

        if (reservation is null)
        {
            throw new Exception("Reservacion no encontrada.");
        }

        return new ReservationDetailsDto
        {
            Id = reservation.Id,
            UserEmail = reservation.User.Email.Value,
            RoomNumber = reservation.Room.RoomNumber
        };
    }
}