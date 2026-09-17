using MediatR;

namespace OmniReserve.Application.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomResponseDto>
{
    public Task<RoomResponseDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        // Se simula la recuperación desde la base de datos transformándolo inmediatamente a DTO
        var response = new RoomResponseDto(
            request.RoomId,
            "101",
            "Single",
            150.00m,
            true
        );

        return Task.FromResult(response);
    }
}