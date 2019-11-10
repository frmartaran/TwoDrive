using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class HTMLModel : FileModel
    {
        public bool ShouldRender { get; set; }

        public override FileModel FromDomain(File entity)
        {
            var asHtml = entity as HTMLFile;
            ShouldRender = asHtml.ShouldRender;
            return base.FromDomain(entity) as HTMLModel;
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
                ParentFolder = this.ParentFolder.ToDomain(),
                Owner = this.Owner.ToDomain(),
                ShouldRender = this.ShouldRender
            };
            if (Id.HasValue)
                file.Id = Id.Value;
            return file;
        }
    }
}
