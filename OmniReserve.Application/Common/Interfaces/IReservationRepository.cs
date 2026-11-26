using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Common.Interfaces;

public interface IReservationRepository
{
    Task AddAsync(Reservation reservation);
    Task<Reservation?> GetByIdAsync(Guid id);
    Task<List<Reservation>> GetByUserIdAsync(Guid userId);
    Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime start, DateTime end);
}