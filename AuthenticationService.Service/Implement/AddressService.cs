﻿using AuthenticationService.Model.Entities.Address;
using AuthenticationService.Model.Repository;
using AuthenticationService.ViewModel.Dtos.Address;
using AuthenticationService.ViewModel.Dtos.User;

namespace AuthenticationService.Service.Implement;

public class AddressService : IAddressService
{
    private IMongoRepository<Address> addressRepository;
    private IMongoRepository<UserAddress> userAddressRepository;
    public AddressService(IMongoRepository<Address> addressRepository, IMongoRepository<UserAddress> userAddressRepository)
    {
        this.userAddressRepository = userAddressRepository;
        this.addressRepository = addressRepository;
    }
    public async Task CreateAddressAsync(AddAddressDto address, string uid)
    {
        var newAddress = new Address()
        {
            AddressType = address.AddressType,
            Addrress = address.Address,
            Lat = address.Lat,
            Lng = address.Lng,
            Information = address.Information
        };

        newAddress = await addressRepository.InsertAsync(newAddress);
        await userAddressRepository.InsertAsync(new UserAddress()
        {
            UserId = uid,
            AddressId = newAddress.Id,
        });

    }

    public async Task DeleteAddressAsync(string addressId, string uid)
    {
        var userAddress = await userAddressRepository.FindOneAsync(x => x.UserId == uid && x.AddressId == addressId);
        if (userAddress == null)
        {
            return;
        }
        await userAddressRepository.RemoveAsync(userAddress.Id);
        await addressRepository.RemoveAsync(userAddress.AddressId);
    }

    public async Task<List<AddressDto>> GetUserAddressAsync(string uid)
    {
        var addressId = userAddressRepository.FindAsync(x => x.UserId == uid).Result.Select(x => x.AddressId);
        var addresses = await addressRepository.FindAsync(x => addressId.Contains(x.Id));

        var rs = new List<AddressDto>();
        foreach (var address in addresses)
        {
            rs.Add(new AddressDto()
            {
                AddressId = address.Id,
                Address = address.Addrress,
                AddressType = address.AddressType,
                Information = address.Information,
                Lat = address.Lat,
                Lng = address.Lng,
            });
        }
        return rs;
    }

    public async Task UpdateAddressAsync(UpdateAddressDto address, string uid)
    {
        if (userAddressRepository.FindAsync(x => x.AddressId == address.AddressId && x.UserId == uid) == null)
        {
            throw new Exception("Something when wrong");
        }
        else
        {
            var old = await addressRepository.FindByIdAsync(address.AddressId);
            old.Information = address.Information;
            old.Lng = address.Lng;
            old.Lat = address.Lat;
            old.Addrress = address.Address;
            old.AddressType = address.AddressType;
            await addressRepository.UpdateAsync(old);
        }
    }
}
