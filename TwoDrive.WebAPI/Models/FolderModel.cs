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
        public ICollection<ElementModel> FolderChildren { get; set; }

        public FolderModel FromDomain(Folder entity)
        {
            throw new NotImplementedException();
        }

        public Folder ToDomain()
        {
            throw new NotImplementedException();
        }
    }
}
