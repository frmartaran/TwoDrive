using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return Ok(token);
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