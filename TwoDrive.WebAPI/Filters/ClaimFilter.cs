
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.WebApi.Filters
{
    public abstract class ClaimFilter : Attribute, IActionFilter
    {
        protected Element Element { get; set; }
        protected ClaimType Action { get; set; }
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
            var sessionLogic = (SessionLogic)context.HttpContext.RequestServices.GetService(typeof(SessionLogic));
            if (!sessionLogic.IsValidToken(token))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 400,
                    Content = "Invalid Token"
                };
                return;
            }
            if (sessionLogic.HasLevel(token))
            {
                AllowAdministrator();
            }
            var writer = sessionLogic.GetWriter(token);
            HasClaims(writer);
        }

        protected abstract void HasClaims(Writer writer);

        protected virtual void AllowAdministrator()
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}