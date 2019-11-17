using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Domain
{
    public class WriterFriend
    {
        public int Id { get; set; }

        public int WriterId { get; set; }

        public virtual Writer Writer { get; set; }

        public int FriendId { get; set; }

        public virtual Writer Friend { get; set; }
    }
}
