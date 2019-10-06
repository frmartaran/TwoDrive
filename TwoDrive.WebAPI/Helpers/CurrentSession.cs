using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TwoDrive.BusinessLogic;
using TwoDrive.Domain;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Helpers
{
    public class CurrentSession : ICurrent
    {
        private ISessionLogic logic;

        public CurrentSession(ISessionLogic sessionLogic)
        {
            logic = sessionLogic;
        }
        public Writer GetCurrentUser(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"];
            return logic.GetWriter(token);
        }

        public Session GetCurrentSession(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"];
            return logic.GetSession(token);
        }
    }
}
