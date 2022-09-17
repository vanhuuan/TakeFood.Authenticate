using AuthenticationService.Model.Entities.Role;
using AuthenticationService.Model.Entities.User;
using AuthenticationService.Model.Repository;
using AuthenticationService.ViewModel.Dtos;
using AuthenticationService.ViewModel.Dtos.User;
using MongoDB.Bson;
using BC = BCrypt.Net.BCrypt;

namespace AuthenticationService.Service.Implement;

public class UserService : IUserService
{
    /// <summary>
    /// UserRepository
    /// </summary>
    private readonly IMongoRepository<User> userRepository;

    /// <summary>
    /// UserRepository
    /// </summary>
    private readonly IMongoRepository<Role> roleRepository;

    /// <summary>
    /// UserRepository
    /// </summary>
    private readonly IMongoRepository<UserRefreshToken> userRefreshTokenRepository;

    /// <summary>
    /// User service constructor
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="roleRepository"></param>
    /// <param name="httpContextAccessor"></param>
    public UserService(IMongoRepository<User> userRepository,
                       IMongoRepository<UserRefreshToken> userRefreshTokenRepository,
                       IMongoRepository<Role> roleRepository)
    {
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.userRefreshTokenRepository = userRefreshTokenRepository;
    }

    /// <summary>
    /// Get user by user id
    /// </summary>
    /// <returns></returns>
    public async Task<UserViewDto> GetUserByIdAsync(string id)
    {
        var user = await userRepository.FindByIdAsync(id);
        var view = new UserViewDto()
        {
            Name = user.Name,
            Email = user.Email,
            Photo = user.Avatar,
            Id = user.Id,
            Phone = user.PhoneNumber

        };
        var role = new List<String>();
        var listRole = await roleRepository.GetAllAsync();
        foreach (var i in listRole)
        {
            if (user.RoleIds.Contains(i.Id))
            {
                role.Add(i.Name!);
            }
        }
        view.Roles = role;
        return view;
    }

    /// <summary>
    /// Create new user
    /// </summary>
    /// <returns></returns>
    public async Task<UserViewDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        var users = await userRepository.FindAsync(x => x.PhoneNumber == createUserDto.PhoneNumber);
        if (users.Count != 0)
        {
            return null;
        }
        users = await userRepository.FindAsync(x => x.Email == createUserDto.Email);
        if (users.Count != 0)
        {
            return null;
        }

        var user = new User()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = createUserDto.Name,
            PhoneNumber = createUserDto.PhoneNumber,
            Email = createUserDto.Email,
            Avatar = createUserDto.Avatar,
            RoleIds = new List<String>() { "2" },
            IsActive = true,
            UpdatedDate = DateTime.Now,
            CreatedDate = DateTime.Now,
            Password = BC.HashPassword(createUserDto.Password),
        };

        var rs = await userRepository.InsertAsync(user);

        return await GetUserByIdAsync(rs.Id);
    }

    /// <summary>
    /// Sign in by phone number or email
    /// </summary>
    /// <returns></returns>
    public async Task<UserViewDto> SignIn(LoginDto loginDto)
    {
        var user = await userRepository.FindOneAsync(x => x.PhoneNumber.Equals(loginDto.UserName) || x.Email.Equals(loginDto.UserName));
        if (user == null)
        {
            return null;
        }
        if (BC.Verify(loginDto.Password, user.Password) && user.IsActive)
        {
            return await GetUserByIdAsync(user.Id);
        }
        else
        {
            return null;
        }
    }
}
