using api.Data.Repositories;
using api.Models.DTOs;
using api.Services.Interfaces;
using api.Utilities;

namespace api.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        JwtTokenGenerator jwtTokenGenerator,
        ILogger<AuthService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
    {
        _logger.LogInformation("Intento de autenticación: {Username}", request.Username);

        var usuario = await _usuarioRepository.GetByUsernameAsync(request.Username);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
        {
            _logger.LogWarning("Autenticación fallida para: {Username}", request.Username);
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        if (usuario.Estado != "Activo")
        {
            _logger.LogWarning("Cuenta bloqueada: {Username}", request.Username);
            throw new UnauthorizedAccessException("La cuenta está bloqueada");
        }

        var token = _jwtTokenGenerator.GenerateToken(usuario);
        _logger.LogInformation("Autenticación exitosa: {Username}", request.Username);

        return new LoginResponse
        {
            Token = token,
            Username = usuario.Username,
            Rol = usuario.Rol?.NombreRol ?? string.Empty
        };
    }
}
