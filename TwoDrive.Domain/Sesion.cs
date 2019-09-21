using System;
namespace TwoDrive.Domain
{
    public class Sesion
    {
        public Guid Token { get; set; }
        public Writer Writer { get; set; }
    }
}