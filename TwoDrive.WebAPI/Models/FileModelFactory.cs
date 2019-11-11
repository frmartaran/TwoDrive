using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.WebApi.Models
{
    public class FileModelFactory
    {
        public static FileModel GetModel(File entity)
        {
            switch (entity.GetType().Name)
            {
                case "HtmlFile":
                    return new HTMLModel().FromDomain(entity);
                case "TxtFile":
                    return new TxtModel().FromDomain(entity);
                default:
                    throw new ArgumentException("");
            }
        }
    }
}
