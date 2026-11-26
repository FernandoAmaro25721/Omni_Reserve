using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Construir Value Object de correo
        var email = new EmailAddress(request.Email);

        // 2. Hashear la contraseña plana antes de crear la entidad del dominio
        string secureHash = _passwordHasher.Hash(request.Password);

        // 3. Crear y persistir la entidad User con el hash resultante
        var user = new User(request.Name, email, secureHash);

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}