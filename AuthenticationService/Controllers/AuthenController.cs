﻿using AuthenticationService.Model.Content;
using AuthenticationService.Service;
using AuthenticationService.ViewModel.Dtos;
using AuthenticationService.ViewModel.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Controllers;


public class AuthenController : Controller
{
    // private string url = "https://localhost:7287/";

    private string url = "https://takefood-authentication.azurewebsites.net/";
    private string urlWeb = "https://takefoodstore.web.app/";
    // private string urlWeb = "https://localhost:3000/";
    public IUserService UserService { get; set; }

    public IJwtService JwtService { get; set; }

    public IMailService MailService { get; set; }

    public AuthenController(IUserService userService, IJwtService jwtService, IMailService mailService)
    {
        UserService = userService;
        JwtService = jwtService;
        MailService = mailService;
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

            var accessToken = JwtService.GenerateSecurityToken(rs.Id, rs.Roles);
            var mail = new MailContent();
            mail.Subject = "Take Food Activation Email";
            mail.To = user.Email;
            mail.Body = $"\r\nHello {user.Name},\r\n\r\nThank you for joining TakeFood.\r\n\r\nWe’d like to confirm that your account was created successfully. To active click the link below.\r\n\r {url + "Active?token=" + accessToken} \r\n\r\nBest,\r\nThe TakeFood team";
            await MailService.SendMail(mail);
            return Ok();
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
            rs.RefreshToken = refreshToken;
            rs.AccessToken = accessToken;
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
            rs.AccessToken = accessToken;
            SetTokenCookie(token, accessToken);
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("Active")]
    public async Task<ActionResult> ActiveUser([Required] string token)
    {
        try
        {
            UserService.Active(token);
            return Ok("OKe roi do, ve lai r dang nhap di");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private void SetTokenCookie(string refreshToken, string accessToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = false,
            Expires = DateTime.UtcNow.AddMonths(5)
        };
        Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
    }

    [HttpGet]
    [Route("ForgetPass")]
    public async Task<ActionResult<UserViewDto>> ForgetpassAsync([FromQuery][Required] string gmail)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await UserService.GetUserByEmail(gmail);
            if (rs == null)
            {
                throw new Exception("User existed");
            }

            var accessToken = JwtService.GenerateRenewToken(rs.Id);
            var mail = new MailContent();
            mail.Subject = "Take Food Renew Password";
            mail.To = rs.Email;
            mail.Body = $"\r\nHello {rs.Name},\r\n\r\nThank you for using TakeFood.\r\n\r\n. To renew your password click the link below.\r\n\r " +
                $"{urlWeb + "changePass?token=" + accessToken}&email={gmail} \r\n\r\nBest,\r\nThe TakeFood team";
            await MailService.SendMail(mail);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("RenewPassword")]
    public async Task<ActionResult<UserViewDto>> RenewPasswordAsync([FromBody] RenewPasswordDto user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = JwtService.ValidRenewToken(user.Token);
            if (!rs)
            {
                BadRequest("Invalid token");
            }
            await UserService.RenewPassword(user);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return BadRequest(e.Message);
        }
    }

    public string GetId()
    {
        String id = HttpContext.Items["Id"]!.ToString()!;
        return id;
    }
    public string GetId(string token)
    {
        return JwtService.GetId(token);
    }
}
