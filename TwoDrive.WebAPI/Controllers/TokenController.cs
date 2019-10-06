using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private ISessionLogic logic;

        public TokenController(ISessionLogic sessionLogic)
        {
            logic = sessionLogic;
        }

        [HttpPost]
        public IActionResult LogIn([FromBody] LogInModel model)
        {
            var token = logic.Create(model.Username, model.Password);
            return Ok(token);
        }
    }
}