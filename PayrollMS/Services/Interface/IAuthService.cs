using PayrollMS.Dtos;

namespace PayrollMS.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedRolesAsync();
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthServiceResponseDto> MakeManagerAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeAccountantAsync(UpdatePermissionDto updatePermissionDto);
    }
}
