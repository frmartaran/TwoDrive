using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Filters
{
    public class ClaimFilter : Attribute, IActionFilter
    {
        protected ClaimType Action { get; set; }

        public ClaimFilter(ClaimType type)
        {
            Action = type;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {

            var token = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 400,
                    Content = ApiResource.MissingToken
                };
                return;
            }
            var sessionLogic = (ISessionLogic) context.HttpContext.RequestServices.GetService(typeof(ISessionLogic));
            if (!sessionLogic.IsValidToken(token))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 400,
                    Content = ApiResource.InvalidToken
                };
                return;
            }
            if ((sessionLogic.HasLevel(token) && !IsAdministratorAllowedAction()) ||
                !sessionLogic.HasLevel(token))
            {
                var element = GetElement(context);
                if (element == null)
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = 404,
                        Content = ApiResource.ElementNotFound_ClaimFilter
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
                        Content = string.Format(ApiResource.NotAllowed_ClaimFilter, 
                        writer.UserName, Action.ToString())
                    };
                    return;
                }
            }
        }

        private Element GetElement(ActionExecutingContext context)
        {
            var elementId = context.RouteData.Values["id"];
            if (elementId == null)
            {
                return null;
            }

            var ElementRepository = (IRepository<Element>) context.HttpContext.RequestServices
                            .GetService(typeof(IRepository<Element>));

            var element = ElementRepository.Get(Convert.ToInt32(elementId));
            return element;
        }

        private bool IsAdministratorAllowedAction()
        {
            return Action == ClaimType.Delete || Action == ClaimType.Read;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}