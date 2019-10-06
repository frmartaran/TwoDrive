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
                return BadRequest("Incorrect username or password");
            }
            return Ok(token);
        }

        [HttpDelete]
        public IActionResult LogOut()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("You are not logged in");
                }
                var session = logic.GetSession(token);
                return Ok("Bye!");
            }
            catch (LogicException)
            {
                return BadRequest("There was an error logging out");
            }
        }
    }
}