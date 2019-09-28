
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TwoDrive.Domain;

namespace TwoDrive.WebApi.Filters
{
    public class AuthorizeFilter : Attribute, IActionFilter
    {
        private Role Role { get; set; }

        public AuthorizeFilter(Role role)
        {
            Role = role;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 400,
                    Content = "Token is required"
                };
                return;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}