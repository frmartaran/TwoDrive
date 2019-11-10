using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Helpers.JsonConverters;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    [JsonConverter(typeof(FileModelConverter))]
    public class FileModel : ElementModel, IModel<File, FileModel>
    {
        public string Content { get; set; }

        public virtual FileModel FromDomain(File entity)
        {
            if (entity == null)
                return null;

            Name = entity.Name;
            CreationDate = entity.CreationDate;
            DateModified = entity.DateModified;
            Content = entity.Content;
            ParentFolder = new FolderModel().FromDomain(entity.ParentFolder);
            Owner = new WriterModel().FromDomain(entity.Owner);
            Id = entity.Id;
            return this;

        }

        public virtual File ToDomain()
        {
            return null;
        }
    }
}
