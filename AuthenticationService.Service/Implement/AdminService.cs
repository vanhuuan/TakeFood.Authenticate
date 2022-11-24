using AuthenticationService.Model.Entities.Address;
using AuthenticationService.Model.Entities.Role;
using AuthenticationService.Model.Entities.User;
using AuthenticationService.Model.Repository;
using AuthenticationService.ViewModel.Dtos.User;
using MongoDB.Driver;

namespace AuthenticationService.Service.Implement;

public class AdminService : IAdminService
{
    /// <summary>
    /// UserRepository
    /// </summary>
    private readonly IMongoRepository<User> userRepository;

    /// <summary>
    /// AccountRepository
    /// </summary>
    private readonly IMongoRepository<Account> accountRepository;

    /// <summary>
    /// RoleRepository
    /// </summary>
    private readonly IMongoRepository<Role> roleRepository;

    /// <summary>
    /// UserRefreshTokenRepository
    /// </summary>
    private readonly IMongoRepository<UserRefreshToken> userRefreshTokenRepository;

    private readonly IMongoRepository<Address> addressRepository;
    /// <summary>
    /// Mail Service
    /// </summary>
    private readonly IMailService mailService;

    /// <summary>
    /// Jwt Service
    /// </summary>
    private readonly IJwtService jwtService;
    private IAddressService addressService;
    private readonly IMongoRepository<UserAddress> userAddressRepository;
    public AdminService(IMongoRepository<User> userRepository,
                       IMongoRepository<UserRefreshToken> userRefreshTokenRepository,
                       IMongoRepository<Role> roleRepository,
                       IMongoRepository<Account> accountRepository,
                       IMailService mailService,
                       IJwtService jwtService,
                       IAddressService addressService,
                       IMongoRepository<Address> addressRepository,
                       IMongoRepository<UserAddress> userAddressRepository)
    {
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.userRefreshTokenRepository = userRefreshTokenRepository;
        this.accountRepository = accountRepository;
        this.mailService = mailService;
        this.jwtService = jwtService;
        this.addressService = addressService;
        this.addressRepository = addressRepository;
    }
    public async Task<UserPagingData> GetPagingUser(GetPagingUserDto getPagingUserDto)
    {
        IList<User> listUser;
        int total = 0;
        if (getPagingUserDto.QueryType == "Email")
        {
            var accounts = await accountRepository.GetPagingAsync(Builders<Account>.Filter.Where(x => x.Email.Contains(getPagingUserDto.QueryString)), getPagingUserDto.PageNumber - 1, getPagingUserDto.PageSize);
            var listAcountsId = accounts.Data.Select(x => x.UserId);
            var users = await userRepository.FindAsync(x => listAcountsId.Contains(x.Id));
            listUser = users;
            total = accounts.Count;
        }
        else
        {
            var filter = CreateUserFilter(getPagingUserDto.QueryString, getPagingUserDto.QueryType);
            var sort = CreateSortFilter(getPagingUserDto.SortBy, getPagingUserDto.SortType);
            var users = await userRepository.GetPagingAsync(filter, getPagingUserDto.PageNumber - 1, getPagingUserDto.PageSize, sort);
            listUser = users.Data.ToList();
            total = users.Count;
        }
        var list = new List<UserCardDto>();
        foreach (var user in listUser)
        {
            var userAddress = addressService.GetUserAddressAsync(user.Id).Result.FirstOrDefault();
            var account = await accountRepository.FindOneAsync(x => x.UserId == user.Id);
            var address = "";
            if (userAddress != null)
            {
                address = userAddress.Address;
            }
            if (account != null)
            {
                list.Add(new UserCardDto()
                {
                    Address = address,
                    UserId = user.Id,
                    Name = user.Name,
                    Email = account.Email,
                    Gender = user.Gender == true ? "Nam" : "Nu",
                    PhoneNumber = user.PhoneNumber,
                    Status = user.State,
                });
            }
        }

        switch (getPagingUserDto.SortBy)
        {
            case "Name":
                list = list.OrderBy(x => x.Name).ToList(); break;
            case "Email":
                list = list.OrderBy(x => x.Email).ToList(); break;
            case "PhoneNumber":
                list = list.OrderBy(x => x.PhoneNumber).ToList(); break;
            default: list = list.OrderBy(x => x.Name).ToList(); break;
        }

        if (getPagingUserDto.SortType == "Desc")
        {
            list.Reverse();
        }
        int stt = 0;
        foreach (var i in list)
        {
            stt++;
            i.Stt = stt;
            i.Id = stt;
        }
        var respone = new UserPagingData()
        {
            Total = total,
            Users = list,
            PageIndex = getPagingUserDto.PageNumber,
            PageSize = getPagingUserDto.PageSize
        };
        return respone;
    }

    private FilterDefinition<User> CreateUserFilter(string query, string queryType)
    {
        var filter = Builders<User>.Filter.Empty;
        if (queryType != "All")
        {
            switch (queryType)
            {
                case "PhoneNumber": filter &= Builders<User>.Filter.Where(x => x.PhoneNumber.Contains(query)); break;
                case "Name": filter &= Builders<User>.Filter.Where(x => x.Name.Contains(query)); break;
                default: filter &= Builders<User>.Filter.StringIn(x => x.Name, query); break;
            }
        }
        filter &= Builders<User>.Filter.Where(x => x.RoleIds.Contains("3"));
        return filter;
    }

    private SortDefinition<User> CreateSortFilter(string sortBy, string sortType)
    {
        var filter = Builders<User>.Sort.Ascending(x => x.Id);
        if (sortType == "Desc")
        {
            switch (sortBy)
            {
                case "PhoneNumber": filter = Builders<User>.Sort.Descending(x => x.PhoneNumber); break;
                case "Name": filter = Builders<User>.Sort.Descending(x => x.Name); break;
                default: filter = Builders<User>.Sort.Descending(x => x.PhoneNumber); break;
            }
        }
        else
        {
            switch (sortBy)
            {
                case "PhoneNumber": filter = Builders<User>.Sort.Ascending(x => x.PhoneNumber); break;
                case "Name": filter = Builders<User>.Sort.Ascending(x => x.Name); break;
                default: filter = Builders<User>.Sort.Ascending(x => x.PhoneNumber); break;
            }
        }
        return filter;
    }

    public async Task MakeAdmin(string userEmail)
    {
        var account = await accountRepository.FindOneAsync(x => x.Email == userEmail);
        if (account == null)
        {
            throw new Exception("User's not exist");
        }
        var user = await userRepository.FindByIdAsync(account.UserId);
        if (user == null)
        {
            throw new Exception("User's not exist");
        }
        if (user.RoleIds.Contains("3"))
        {
            return;
        }
        user.RoleIds.Add("3");
        await userRepository.UpdateAsync(user);
    }

    public async Task RemoveAdmin(string adminId)
    {
        var account = await accountRepository.FindOneAsync(x => x.Email == adminId);
        if (account == null)
        {
            throw new Exception("User's not exist");
        }
        var user = await userRepository.FindByIdAsync(account.UserId);
        if (user == null)
        {
            throw new Exception("User's not exist");
        }
        user.RoleIds.Remove("3");
        await userRepository.UpdateAsync(user);
    }
}
