using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class ClaimModel : IModel<Claim, ClaimModel>
    {
        public ClaimType Type { get; set; }
        public Element Element { get; set; }
        public Claim ToDomain()
        {
            return new Claim
            {
                Type = this.Type,
                Element = this.Element
            };
        }

        public ClaimModel FromDomain(Claim entity)
        {
            Type = entity.Type;
            Element = entity.Element;
            return this;
        }
    }
}
