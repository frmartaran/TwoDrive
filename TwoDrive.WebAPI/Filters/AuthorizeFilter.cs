
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Logic;
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
        }

        public void OnActionExecuting(ActionExecutingContext context)
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
            var sessionLogic = (ISessionLogic) context.HttpContext.RequestServices.GetService(typeof(ISessionLogic));
            if (!sessionLogic.IsValidToken(token))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 400,
                    Content = "Invalid Token"
                };
                return;
            }
            if (!sessionLogic.HasLevel(token))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 400,
                    Content = "The user is no authorized"
                };
                return;
            }
        }
    }
}