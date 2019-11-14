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
        public ElementModel Element { get; set; }
        public CustomClaim ToDomain()
        {
            return new CustomClaim
            {
                Type = this.Type,
                Element = GetElementDomain(this.Element)
            };
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

        public ClaimModel FromDomain(CustomClaim entity)
        {
            Type = entity.Type;
            Element = GetElementModel(entity.Element);
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
