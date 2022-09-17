using AuthenticationService.ViewModel.Dtos;
using AuthenticationService.ViewModel.Dtos.User;

namespace AuthenticationService.Service;

/// <summary>
/// IUserService
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Get user by user id
    /// </summary>
    /// <returns></returns>
    Task<UserViewDto> GetUserByIdAsync(string id);

    /// <summary>
    /// Create new user
    /// </summary>
    /// <returns></returns>
    Task<UserViewDto> CreateUserAsync(CreateUserDto createUserDto);

    /// <summary>
    /// Sign in by phone number or email
    /// </summary>
    /// <returns></returns>
    Task<UserViewDto> SignIn(LoginDto loginDto);
}
