using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<Room>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IDistributedCache _cache;
    private const string CacheKey = "rooms_catalog";

    public GetAllRoomsQueryHandler(IRoomRepository roomRepository, IDistributedCache cache)
    {
        _roomRepository = roomRepository;
        _cache = cache;
    }

    public async Task<List<Room>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        // 1. Verificar caché (Cache Hit)
        var cachedData = await _cache.GetStringAsync(CacheKey, cancellationToken);
        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedRooms = JsonSerializer.Deserialize<List<Room>>(cachedData);
            if (cachedRooms is not null)
            {
                return cachedRooms;
            }
        }

        // 2. Consultar base de datos (Cache Miss)
        var rooms = await _roomRepository.GetAllAsync(cancellationToken);

        // 3. Guardar en Redis con TTL para evitar datos obsoletos
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        var serializedRooms = JsonSerializer.Serialize(rooms);
        await _cache.SetStringAsync(CacheKey, serializedRooms, cacheOptions, cancellationToken);

        // 4. Retornar resultado
        return rooms;
    }
}