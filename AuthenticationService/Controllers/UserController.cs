using AuthenticationService.Middleware;
using AuthenticationService.Model.Entities;
using AuthenticationService.Service;
using AuthenticationService.ViewModel.Dtos;
using AuthenticationService.ViewModel.Dtos.User;
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
    [Authorize]
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
    [Authorize]
    [Route("UpdateAddress")]
    public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressDto address)
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

    [HttpDelete]
    [Authorize]
    [Route("DeleteAdderss")]
    public async Task<IActionResult> DeleteAddressAsync(string addressId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await addressService.DeleteAddressAsync(addressId, GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize(roles: Roles.User)]
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
    
    [HttpGet]
    [Route("GetNewsUser")]
    public async Task<JsonResult> GetNewsUser()
    {
        try
        {
            JsonResult json = new JsonResult(await userService.GetNewsUser());
            return json;
        }
        catch (Exception e)
        {
            return new JsonResult(e);
        }
    }

    [HttpGet]
    [Authorize(roles: Roles.Admin)]
    [Route("GetPagingUser")]
    public async Task<IActionResult> GetPagingUserAsync([FromQuery] GetPagingUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await userService.GetPagingUser(dto);
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("GetAllUser")]
    public async Task<JsonResult> GetAllUser([Optional] string status)
    {
        try
        {
            List<ShowUserDto> showUsers = await userService.GetAllUser(status);
            return new JsonResult(showUsers);
        }
        catch (Exception e)
        {
            return new JsonResult(e);
        }
    }

    [HttpGet]
    [Route("GetUserByID")]
    public async Task<JsonResult> GetUserByID(string id)
    {
        try
        {
            DetailsUserDto detailsUserDto = await userService.GetUserByID(id);
            return new JsonResult(detailsUserDto);
        }
        catch
        {
            return new JsonResult("Không tồn tại user này");
        }
    }

    [HttpGet]
    [Route("FilterUser")]
    public async Task<JsonResult> FilterUserByKey(string status, string key)
    {
        try
        {
            IEnumerable<ShowUserDto> showUserDtos = await userService.FilterByKey(status, key);
            return new JsonResult(showUserDtos);
        }
        catch (Exception e)
        {
            return new JsonResult(e);
        }
    }

    [HttpDelete]
    [Route("DeleteUser")]
    public async Task<IActionResult> DeleteUser([Required] string id)
    {
        try
        {
            await userService.DeleteUser(id);
            return Ok();
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
    }
    [HttpPut]
    [Authorize(roles: Roles.Admin)]
    [Route("ChangeUserStatus")]
    public async Task<IActionResult> ChangeUserStatus([Required][FromQuery] string id)
    {
        try
        {
            await userService.ChangeUserStatus(id);
            return Ok();
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
    }

    [HttpGet]
    [Authorize(roles: Roles.Admin)]
    [Route("GetPagingAdmin")]
    public async Task<IActionResult> GetPagingAdminAsync([FromQuery] GetPagingUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await adminService.GetPagingUser(dto);
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Authorize(roles: Roles.Admin)]
    [Route("MakeAdmin")]
    public async Task<IActionResult> MakeAdmin([FromQuery] string userEmail)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await adminService.MakeAdmin(userEmail);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Authorize(roles: Roles.Admin)]
    [Route("RemoveAdmin")]
    public async Task<IActionResult> RemoveAdmin([FromQuery] string userId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await adminService.RemoveAdmin(userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    public string GetId()
    {
        String id = HttpContext.Items["Id"]!.ToString()!;
        return id;
    }
}

