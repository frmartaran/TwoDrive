using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TwoDrive.Domain;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Models
{
    public class WriterModel : IModel<Writer, WriterModel>
    {
        public int? Id { get; set; }

        public Role Role { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICollection<Writer> Friends { get; set; }

        public ICollection<Claim> Claims { get; set; }

        public WriterModel FromDomain(Writer entity)
        {
            Id = entity.Id;
            Role = entity.Role;
            UserName = entity.UserName;
            Password = entity.Password;
            Friends = entity.Friends;
            Claims = entity.Claims;
            return this;
        }

        public Writer ToDomain()
        {
            var writer = new Writer
            {
                Role = this.Role,
                UserName = this.UserName,
                Password = this.Password,
                Friends = this.Friends,
                Claims = this.Claims
            };

            if (Id.HasValue)
                writer.Id = Id.Value;

            return writer;
        }
    }
}