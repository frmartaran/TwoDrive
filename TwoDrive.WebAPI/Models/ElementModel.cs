using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoDrive.WebApi.Models
{
    public abstract class ElementModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int? ParentFolderId { get; set; }

        public FolderModel ParentFolder { get; set; }

        public int? OwnerId { get; set; }

        public string OwnerName { get; set; }

        public WriterModel Owner { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsFolder { get; set; }

    }
}
