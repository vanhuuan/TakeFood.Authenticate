﻿using AuthenticationService.Middleware;
using AuthenticationService.Service;
using AuthenticationService.ViewModel.Dtos;
using AuthenticationService.ViewModel.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Controllers;

public class AuthenController : Controller
{
    public IUserService UserService { get; set; }

    public IJwtService JwtService { get; set; }

    public AuthenController(IUserService userService, IJwtService jwtService)
    {
        UserService = userService;
        JwtService = jwtService;
    }

    [HttpPost]
    [Route("SignUp")]
    public async Task<ActionResult<UserViewDto>> SignUpAsync([FromBody] CreateUserDto user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await UserService.CreateUserAsync(user);
            if (rs == null)
            {
                throw new Exception("User existed");
            }
            var refreshToken = JwtService.GenerateRefreshToken(rs.Id);
            var accessToken = JwtService.GenerateSecurityToken(rs.Id, rs.Roles);
            SetTokenCookie(refreshToken, accessToken);
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("SignIn")]
    public async Task<ActionResult<UserViewDto>> SignInAsync([FromBody] LoginDto user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await UserService.SignIn(user);
            if (rs == null)
            {
                throw new Exception("Wrong user name or password");
            }
            var refreshToken = JwtService.GenerateRefreshToken(rs.Id);
            var accessToken = JwtService.GenerateSecurityToken(rs.Id, rs.Roles);
            SetTokenCookie(refreshToken, accessToken);
            return Ok(rs);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
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
            var rs = await UserService.GetUserByIdAsync(id);
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("GetAccessToken")]
    public async Task<ActionResult> GetAccessTokenAsync([Required] string token)
    {
        try
        {
            if (!JwtService.ValidRefreshToken(token))
            {
                return BadRequest("Token expired");
            }
            var id = GetId(token);
            var rs = await UserService.GetUserByIdAsync(id);
            var accessToken = JwtService.GenerateSecurityToken(id, rs.Roles);
            SetTokenCookie(token, accessToken);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /*    [HttpGet]
        [Route("Active")]
        public async Task<ActionResult> ActiveUser([Required] string token)
        {
            try
            {
                await UserService.Active(token);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/

    private void SetTokenCookie(string refreshToken, string accessToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMonths(5)
        };
        Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
    }


    public string GetId()
    {
        String token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
        return JwtService.GetId(token);
    }
    public string GetId(string token)
    {
        return JwtService.GetId(token);
    }
}
