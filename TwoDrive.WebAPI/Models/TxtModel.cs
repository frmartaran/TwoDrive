using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class TxtModel : FileModel, IModel<TxtFile, TxtModel>
    {
        public string Content { get; set; }
        public TxtModel FromDomain(TxtFile entity)
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

        public TxtFile ToDomain()
        {
            if (this == null)
                return null;

            var file = new TxtFile
            {
                Name = this.Name,
                CreationDate = this.CreationDate,
                DateModified = this.DateModified,
                Content = this.Content
            };

            if (ParentFolder != null)
                file.ParentFolder = ParentFolder.ToDomain();

            if (Owner != null)
                file.Owner = Owner.ToDomain();

            if (Id.HasValue)
                file.Id = Id.Value;

            return file;
        }
    }
}
