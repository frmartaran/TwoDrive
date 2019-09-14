using System;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.Domain
{
    public class Claim
    {
        public Element Element { get; set; }
        public ClaimType Type { get; set; }
    }
}