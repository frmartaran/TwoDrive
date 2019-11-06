using System;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.Domain
{
    public class CustomClaim
    {
        public int Id { get; set;}
        public Element Element { get; set; }
        public int ElementId { get; set; }
        public ClaimType Type { get; set; }
    }
}