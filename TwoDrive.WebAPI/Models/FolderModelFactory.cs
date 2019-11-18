using System;
using System.Collections.Generic;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Models
{
    public class FolderModelFactory
    {
        public static ElementModel GetModel(Element entity)
        {
            if (entity is File file)
            {
                return new FileModel().FromDomain(file);
            }
            else if(entity is Folder folder)
            {
                return new FolderModel().FromDomain(folder);
            }
            else
            {
                throw new ArgumentException(ApiResource.UnsupportedElementType);
            }
        }

        public static ICollection<ElementModel> GetModelForAllChildren(Folder folder)
        {
            var folderChildren = new List<ElementModel>();
            if(folder.FolderChildren != null)
            {
                foreach (var child in folder.FolderChildren)
                {
                    folderChildren.Add(GetModel(child));
                }
            }
            return folderChildren;
        }
    }
}
