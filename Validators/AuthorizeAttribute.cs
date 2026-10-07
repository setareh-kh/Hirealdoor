using Hirealdoor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hirealdoor.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        // public int[] Type { get; set; } = [];

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items["user"] is not int user)
            {
                // not logged in
                context.Result = new JsonResult(new { Success = false, Message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
            }

            /*
            if (user is { Active: false })
            {
                context.Result = new JsonResult(new { Success = false, Message = "AccessDenied" })
                    { StatusCode = StatusCodes.Status403Forbidden };
            }
            */
        }
    }
}