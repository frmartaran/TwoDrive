using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class FolderModel : ElementModel, IModel<Folder, FolderModel>
    {
        public FolderModel FromDomain(Folder entity)
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
            return this;
        }

        public Folder ToDomain()
        {
            if (this == null)
                return null;
            
            var folder = new Folder
            {
                Name = this.Name,
                CreationDate = this.CreationDate,
                DateModified = this.DateModified
            };

            if (ParentFolder != null)
                folder.ParentFolder = ParentFolder.ToDomain();

            if (Owner != null)
                folder.Owner = Owner.ToDomain();

            if (Id.HasValue)
                folder.Id = Id.Value;

            return folder;
        }

        public Folder ToDomain(Folder folder)
        {
            if (this == null)
                return null;

            if (folder == null)
                return null;

            folder.Name = this.Name;
            folder.CreationDate = this.CreationDate;
            folder.DateModified = this.DateModified;

            if (ParentFolder != null)
                folder.ParentFolder = ParentFolder.ToDomain();

            if (Owner != null)
                folder.Owner = Owner.ToDomain();

            if (Id.HasValue)
                folder.Id = Id.Value;

            return folder;
        }
    }
}
