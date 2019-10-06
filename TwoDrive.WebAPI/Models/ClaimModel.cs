﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.WebApi.Models
{
    public class ClaimModel : Model<Claim, ClaimModel>
    {
        public ClaimType Type { get; set; }
        public Element Element { get; set; }
        protected override Claim ToEntity(ClaimModel model)
        {
            return new Claim
            {
                Type = this.Type,
                Element = this.Element
            };
        }

        protected override ClaimModel ToModel(Claim entity)
        {
            Type = entity.Type;
            Element = entity.Element;
            return this;
        }
    }
}
