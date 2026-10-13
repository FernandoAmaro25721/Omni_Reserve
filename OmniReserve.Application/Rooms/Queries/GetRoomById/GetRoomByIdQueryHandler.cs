using MediatR;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomResponseDto>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomByIdQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomResponseDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Buscar en Repositorio
        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        
        if (room is null)
            throw new Exception("Habitación no encontrada");

        // 2. Mapear a DTO (Ocultar Entidad)
        var response = new RoomResponseDto(
            room.Id, 
            room.RoomNumber, 
            room.Type.ToString(), 
            room.PricePerNight, 
            room.IsAvailable
        );

        return response;
    }
}