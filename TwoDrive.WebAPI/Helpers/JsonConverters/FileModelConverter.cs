using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TwoDrive.WebApi.Models;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Helpers.JsonConverters
{
    public class FileModelConverter : JsonCreationConverter<FileModel>
    {
        protected override FileModel Create(Type objectType, JObject jObject)
        {
            if (jObject == null)
                throw new ArgumentNullException();

            var type = jObject[ApiConstants.DifferentialAttribute].ToString().ToUpper();
            if (type.Equals(ApiConstants.HTMLType))
                return new HTMLModel();
            else if (type.Equals(ApiConstants.TXTType))
                return new TxtModel();
            else
                throw new ArgumentException(ApiResource.UnsupportedFileType);
        }
    }
}
