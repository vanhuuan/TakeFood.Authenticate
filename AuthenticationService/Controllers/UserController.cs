using AuthenticationService.Service;
using AuthenticationService.ViewModel.Dtos;
using AuthenticationService.ViewModel.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers;

public class UserController : ControllerBase
{
    private IAddressService addressService;
    private IUserService userService;
    private IJwtService jwtService { get; set; }
    public UserController(IAddressService addressService, IJwtService jwtService, IUserService userService)
    {
        this.addressService = addressService;
        this.jwtService = jwtService;
        this.userService = userService;
    }

    [HttpPost]
    [Authorize("User")]
    [Route("AddAddress")]
    public async Task<IActionResult> AddNewAddress([FromBody] AddAddressDto address)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await addressService.CreateAddressAsync(address, GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Authorize("User")]
    [Route("UpdateAddress")]
    public async Task<IActionResult> UpdateNewAddress([FromBody] UpdateAddressDto address)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await addressService.UpdateAddressAsync(address, GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize("User")]
    [Route("GetAddress")]
    public async Task<ActionResult<IList<AddAddressDto>>> GetUserAddress()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var list = await addressService.GetUserAddressAsync(GetId());
            return Ok(list);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize("User")]
    [Route("GetUserInfo")]
    public async Task<ActionResult<UserViewDto>> GetUserAsync()
    {
        try
        {
            var id = GetId();
            var rs = await userService.GetUserByIdAsync(id);
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Authorize("User")]
    [Route("UpdateInfo")]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserDto info)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var dto = await userService.UpdateUserInfo(info, GetId());
            return Ok(dto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    public string GetId()
    {
        String token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
        return jwtService.GetId(token);
    }
    public string GetId(string token)
    {
        return jwtService.GetId(token);
    }
}
