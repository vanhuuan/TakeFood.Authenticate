using AuthenticationService.ViewModel.Dtos.User;

namespace AuthenticationService.Service;

public interface IAdminService
{
    Task<UserPagingData> GetPagingUser(GetPagingUserDto getPagingUserDto);
    Task MakeAdmin(string userEmail);
    Task RemoveAdmin(string adminId);
}
