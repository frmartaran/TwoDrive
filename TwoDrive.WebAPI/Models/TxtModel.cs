using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class TxtModel : FileModel
    {
        public string Content { get; set; }
        public override FileModel FromDomain(File entity)
        {
            var file = entity as TxtFile;
            Content = file.Content;
            return base.FromDomain(file);
        }
        public File ToDomain(TxtFile file)
        {
            if (this == null)
                return null;

            if (file == null)
                return null;

            file.Name = this.Name;
            file.CreationDate = this.CreationDate;
            file.DateModified = this.DateModified;
            file.Content = this.Content;

            if (ParentFolder != null)
                file.ParentFolder = ParentFolder.ToDomain();

            if (Owner != null)
                file.Owner = Owner.ToDomain();

            if (Id.HasValue)
                file.Id = Id.Value;

            return file;
        }
        public override File ToDomain()
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
