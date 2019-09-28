using System;
namespace TwoDrive.Domain
{
    public class Session
    {
        public int Id { get; set; }
        public Guid Token { get; set; }
        public Writer Writer { get; set; }
    }
}