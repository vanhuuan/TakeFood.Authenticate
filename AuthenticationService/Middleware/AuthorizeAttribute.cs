using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthenticationService.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly String _role;

    public AuthorizeAttribute(String role)
    {
        _role = role ?? "";
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            var check = context.HttpContext.Items["TokenExp"];
            if (Convert.ToBoolean(check) == true)
            {
                context.Result = new JsonResult(new { message = "Unauthorized, Token Exp" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            var role = (List<string>)context.HttpContext.Items["Role"]!;
            if (_role.Any() && !role.Contains(_role))
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        catch (Exception)
        {
            context.Result = new JsonResult(new { message = "Unauthorized, Not Sign Up" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
