using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class SessionModel
    {
        public int UserId { get; set; }

        public Guid Token { get; set; }

        public bool IsAdmin { get; set; }

        public string Username { get; set; }
    }
}
