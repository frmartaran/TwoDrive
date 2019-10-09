
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.WebApi.Filters
{
    public class ClaimFilter : Attribute, IActionFilter
    {
        protected ClaimType Action { get; set; }

        public ClaimFilter(ClaimType type)
        {
            Action = type;
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
            var sessionLogic = (SessionLogic) context.HttpContext.RequestServices.GetService(typeof(SessionLogic));
            if (!sessionLogic.IsValidToken(token))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 400,
                    Content = "Invalid Token"
                };
                return;
            }
            if (!(sessionLogic.HasLevel(token) && IsAdministratorAllowedAction()))
            {
                var element = GetElement(context);
                if (element == null)
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = 400,
                        Content = "Invalid Element"
                    };
                    return;
                }
                var writer = sessionLogic.GetWriter(token);
                var hasClaim = writer.HasClaimsFor(element, Action);
                if (!hasClaim)
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = 400,
                        Content = $"This user is not allow to {Action.ToString()} this element"
                    };
                    return;
                }
            }
        }

        private Element GetElement(ActionExecutedContext context)
        {
            var elementId = context.RouteData.Values["id"];
            if (elementId == null)
            {
                return null;
            }

            var ElementRepository = (IRepository<Element>) context.HttpContext.RequestServices
                            .GetService(typeof(IRepository<Element>));

            var element = ElementRepository.Get((int) elementId);
            return element;
        }

        private bool IsAdministratorAllowedAction()
        {
            return Action == ClaimType.Delete || Action == ClaimType.Read;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}