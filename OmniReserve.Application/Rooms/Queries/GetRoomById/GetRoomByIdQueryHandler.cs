using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Application.Rooms.Queries.GetRoomById;

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
        var room = await _roomRepository.GetByIdAsync(request.RoomId);

        if (room == null)
        {
            throw new Exception("Room not found");
        }

        return new RoomResponseDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity
        };
    }
}