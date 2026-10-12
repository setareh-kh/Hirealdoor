using Hirealdoor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hirealdoor.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public int[] Type { get; set; } = [];

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["user"] as User;

            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { Success = false, Message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
            }


            if (user is { Active: false })
            {
                context.Result = new JsonResult(new { Success = false, Message = "AccessDenied" })
                    { StatusCode = StatusCodes.Status403Forbidden };
            }

            if (user == null || Type.Length == 0) return;

            if (!Type.Contains((int)user.Type!))
            {
                context.Result = new JsonResult(new { Success = false, Message = "AccessDenied" })
                    { StatusCode = StatusCodes.Status403Forbidden };
            }
        }
    }
}