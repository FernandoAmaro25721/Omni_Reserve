using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IReservationRepository _reservationRepository;

    public CreateReservationCommandHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar disponibilidad antes de crear o persistir la entidad
        bool isAvailable = await _reservationRepository.IsRoomAvailableAsync(
            request.RoomId,
            request.StartDate,
            request.EndDate);

        if (!isAvailable)
        {
            throw new InvalidOperationException("La habitación seleccionada ya está ocupada en el rango de fechas solicitado.");
        }

        // 2. Crear y agregar la reserva solo cuando hay disponibilidad
        var reservation = new Reservation(
            request.RoomId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        await _reservationRepository.AddAsync(reservation);

        return reservation.Id;
    }
}