using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain;

namespace TwoDrive.WebApi.Interfaces
{
    public interface ICurrent
    {
        Writer GetCurrentUser(HttpContext context);

        Session GetCurrentSession(HttpContext context);
    }
}
