using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.WebApi.Models
{
    public class HTMLModel : FileModel
    {
        public bool ShouldRender { get; set; }

        public override FileModel FromDomain(File entity)
        {
            var htmlFile = entity as HTMLFile;
            ShouldRender = htmlFile.ShouldRender;
            return base.FromDomain(entity);
        }

        public File ToDomain(HTMLFile file)
        {
            if (this == null)
                return null;

            if (file == null)
                return null;

            file.Name = this.Name;
            file.CreationDate = this.CreationDate;
            file.DateModified = this.DateModified;
            file.Content = this.Content;
            file.ShouldRender = this.ShouldRender;

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

            var file = new HTMLFile
            {
                Name = this.Name,
                CreationDate = this.CreationDate,
                DateModified = this.DateModified,
                Content = this.Content,
                ShouldRender = this.ShouldRender
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
