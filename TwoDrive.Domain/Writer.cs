using System;
using System.Collections.Generic;

namespace TwoDrive.Domain
{
    public class Writer
    {
        public Guid Id { get; set; }

        public Guid Token { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICollection<Writer> Friends { get; set; }
        
        public ICollection<Claim> Claims { get; set; }
    }
}
