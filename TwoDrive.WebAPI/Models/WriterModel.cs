using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TwoDrive.Domain;

namespace TwoDrive.WebApi.Models
{
    public class WriterModel : Model<Writer, WriterModel>
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<Writer> Friends { get; set; }
        public ICollection<Claim> Claims { get; set; }
        protected override Writer ToEntity(WriterModel model)
        {
            return new Writer
            {
                Id = this.Id,
                Role = this.Role,
                UserName = this.UserName,
                Password = this.Password,
                Friends = this.Friends,
                Claims = this.Claims
            };
        }
        protected override WriterModel ToModel(Writer entity)
        {
            Id = entity.Id;
            Role = entity.Role;
            UserName = entity.UserName;
            Password = entity.Password;
            Friends = entity.Friends;
            Claims = entity.Claims;
            return this;
        }
    }
}