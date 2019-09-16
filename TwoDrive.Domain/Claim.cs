using System;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.Domain
{
    public class Claim
    {
        public int Id { get; set;}
        public Element Element { get; set; }
        public ClaimType Type { get; set; }
    }
}