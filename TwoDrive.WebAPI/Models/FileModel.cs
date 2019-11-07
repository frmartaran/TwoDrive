using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class FileModel : ElementModel, IModel<File, FileModel>
    {
        public string Content { get; set; }

        public virtual FileModel FromDomain(File entity)
        {
            if (entity == null)
                return null;

            Id = entity.Id;
            Name = entity.Name;
            Owner = new WriterModel().FromDomain(entity.Owner);
            OwnerId = entity.OwnerId;
            ParentFolder = new FolderModel().FromDomain(entity.ParentFolder);
            ParentFolderId = entity.ParentFolderId;
            CreationDate = entity.CreationDate;
            DateModified = entity.DateModified;
            Content = entity.Content;
            return this;
        }
        public virtual File ToDomain()
        {
            return null;
        }
    }
}
