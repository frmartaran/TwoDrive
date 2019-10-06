using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TwoDrive.Domain;

namespace TwoDrive.WebApi.Models
{
    public class WriterModel : Model<Writer, WriterModel>
    {
        public int? Id { get; set; }
        public Role Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<WriterModel> Friends { get; set; }
        public ICollection<ClaimModel> Claims { get; set; }
        protected override Writer ToEntity(WriterModel model)
        {
            var writer = new Writer
            {
                Role = this.Role,
                UserName = this.UserName,
                Password = this.Password,
                Friends = AllToEntity(model.Friends),
                Claims = ClaimModel.AllToEntity(model.Claims)
            };

            if (Id.HasValue)
                writer.Id = Id.Value;
                
            return writer;
        }
        protected override WriterModel ToModel(Writer entity)
        {
            Id = entity.Id;
            Role = entity.Role;
            UserName = entity.UserName;
            Password = entity.Password;
            Friends = AllToModel(entity.Friends);
            Claims = ClaimModel.AllToModel(entity.Claims);
            return this;
        }
    }
}