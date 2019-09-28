
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.Domain;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class AuthorizeFilterTest
    {
        [TestMethod]
        public void IsNotAuthorizeNullToken()
        {
            var mockHttpContext = new Mock<ActionExecutedContext>(MockBehavior.Strict);
            var response = new ContentResult
            {
                StatusCode = 400,
                Content = "Token is required"
            };
            mockHttpContext.Setup(m => m.HttpContext.Request.Headers["Authorization"])
            .Returns("");
            mockHttpContext.Setup(m => m.Result)
            .Returns(response);
            var filter = new AuthorizeFilter(Role.Administrator);
            filter.OnActionExecuted(mockHttpContext.Object);

            mockHttpContext.VerifyAll();

        }
    }
}