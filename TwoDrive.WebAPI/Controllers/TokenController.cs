using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class TokenController : ControllerBase
    {
        private ISessionLogic logic;

        private ICurrent currentSession;

        public TokenController(ISessionLogic sessionLogic, ICurrent session)
        {
            logic = sessionLogic;
            currentSession = session;
        }

        [HttpPost]
        public IActionResult LogIn([FromBody] LogInModel model)
        {
            
            var token = logic.Create(model.Username, model.Password);
            if (token == null)
            {
                return BadRequest(ApiResource.LoginError_TokenController);
            }
            var stringToken = token.Value.ToString();
            var session = logic.GetSession(stringToken);
            var sessionModel = new SessionModel
            {
                UserId = session.Writer.Id,
                Token = session.Token,
                IsAdmin = session.Writer.Role == Domain.Role.Administrator,
                Username = session.Writer.UserName
            };
            return Ok(sessionModel);
        }

        [HttpDelete]
        public IActionResult LogOut()
        {
            try
            {
                var session = currentSession.GetCurrentSession(HttpContext);
                logic.RemoveSession(session);
                return Ok(ApiResource.LogOut_TokenController);
            }
            catch (LogicException)
            {
                return BadRequest(ApiResource.LogOutError_TokenController);
            }
        }
    }
}