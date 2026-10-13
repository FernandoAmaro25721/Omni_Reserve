using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
{
    private readonly IRoomRepository _roomRepository;

    public CreateRoomCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        // 1. Instanciar Dominio Rico
        var room = new Room(request.RoomNumber, request.Type, request.PricePerNight);
        
        // 2. Persistir en repositorio
        await _roomRepository.AddAsync(room);
        
        // 3. Devolver resultado
        return room.Id;
    }
}