using api.Models.DTOs;

namespace api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> AuthenticateAsync(LoginRequest request);
}
