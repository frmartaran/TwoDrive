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
            ParentFolderId = entity.ParentFolderId;            
            if (entity.Owner != null)
            {
                OwnerId = entity.Owner.Id;
                OwnerName = entity.Owner.UserName;
            }
            Id = entity.Id;
            IsFolder = false;
            return this;
        }

        public virtual File ToDomain()
        {
            return null;
        }
    }
}
