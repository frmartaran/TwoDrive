using System;
using System.Collections.Generic;

namespace TwoDrive.Domain
{
    public class Writer
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<Writer> Friends { get; set; }
        public ICollection<CustomClaim> Claims { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var toWriter = (Writer)obj;
            return Id == toWriter.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
