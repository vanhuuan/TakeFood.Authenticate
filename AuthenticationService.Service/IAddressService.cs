using AuthenticationService.ViewModel.Dtos.Address;
using AuthenticationService.ViewModel.Dtos.User;

namespace AuthenticationService.Service;

public interface IAddressService
{
    Task<List<AddressDto>> GetUserAddressAsync(string uid);

    Task CreateAddressAsync(AddAddressDto address, string uid);

    Task UpdateAddressAsync(UpdateAddressDto address, string uid);
}
