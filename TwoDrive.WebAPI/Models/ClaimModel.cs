using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class ClaimModel : IModel<CustomClaim, ClaimModel>
    {
        public ClaimType Type { get; set; }
        public Element Element { get; set; }
        public CustomClaim ToDomain()
        {
            return new CustomClaim
            {
                Type = this.Type,
                Element = this.Element
            };
        }

        public ClaimModel FromDomain(CustomClaim entity)
        {
            Type = entity.Type;
            Element = entity.Element;
            return this;
        }
    }
}
