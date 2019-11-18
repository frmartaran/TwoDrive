using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class ClaimModel
    {
        public ICollection<ClaimType> Types { get; set; }
        public ElementModel Element { get; set; }
        public ICollection<CustomClaim> ToDomain()
        {
            var claims = new List<CustomClaim>();
            if(Types != null)
            {
                foreach (var type in Types)
                {
                    claims.Add(new CustomClaim
                    {
                        Type = type,
                        Element = GetElementDomain(this.Element)
                    });
                }
            }
            return claims;
        }

        private Element GetElementDomain(ElementModel element)
        {
            Element modelToReturn = null;
            if (element is FolderModel folder)
            {
                modelToReturn = folder.ToDomain();
            }
            else if (element is FileModel file)
            {
                modelToReturn = file.ToDomain();
            }
            return modelToReturn;
        }

        public ClaimModel FromDomain(IGrouping<int,CustomClaim> claims)
        {
            if (claims != null)
            {
                var claimsByElement = claims
                    .Select(c => c)
                    .FirstOrDefault();
                if(claimsByElement != null)
                {
                    Element = GetElementModel(claimsByElement.Element);
                    Types = claims
                        .Select(c => c.Type)
                        .ToList();
                }                
            }            
            return this;
        }

        private ElementModel GetElementModel(Element element)
        {
            ElementModel modelToReturn = null;
            if(element is Folder folder)
            {
                modelToReturn = new FolderModel().FromDomain(folder);
            }
            else if (element is File file)
            {
                modelToReturn = new FileModel().FromDomain(file);
            }
            return modelToReturn;
        }
    }
}
