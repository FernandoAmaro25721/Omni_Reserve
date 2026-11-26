using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Convertir y validar el correo como Value Object
        var emailAddress = new EmailAddress(request.Email);

        // 2. Buscar al usuario únicamente por correo (nunca comparar texto plano contra el hash en BD)
        var user = await _userRepository.GetByEmailAsync(emailAddress);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        // 3. Verificar la contraseña en texto plano contra PasswordHash
        bool isValidPassword = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isValidPassword)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        // 4. Generar y retornar el token JWT
        return _jwtProvider.Generate(user);
    }
}