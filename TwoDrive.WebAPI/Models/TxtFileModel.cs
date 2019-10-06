using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class TxtFileModel : FileModel, IModel<TxtFile, TxtFileModel>
    {
        public string Content { get; set; }

        public TxtFileModel FromDomain(TxtFile entity)
        {
            throw new NotImplementedException();
        }

        public TxtFile ToDomain()
        {
            throw new NotImplementedException();
        }
    }
}
