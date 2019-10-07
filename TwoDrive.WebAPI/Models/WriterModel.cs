using System;
using System.Collections.Generic;
using System.Linq;
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

        public ICollection<WriterModel> Friends { get; set; }

        public ICollection<ClaimModel> Claims { get; set; }

        public WriterModel FromDomain(Writer entity)
        {
            if (entity == null)
                return null;

            Id = entity.Id;
            Role = entity.Role;
            UserName = entity.UserName;
            Password = entity.Password;

            if(Friends != null)
            {
                Friends = entity.Friends
                .Select(e => new WriterModel().FromDomain(e))
                .ToList();
            }
            if(Claims != null)
            {
                Claims = entity.Claims
                .Select(c => new ClaimModel().FromDomain(c))
                .ToList();
            }

            return this;
        }

        public Writer ToDomain()
        {
            if (this == null)
                return null;

            var writer = new Writer
            {
                Role = this.Role,
                UserName = this.UserName,
                Password = this.Password,
                Friends = this.Friends?
                            .Select(f => f.ToDomain())
                            .ToList() ?? null,
                Claims = this.Claims?
                        .Select(c => c.ToDomain())
                        .ToList() ?? null
            };

            if (Id.HasValue)
                writer.Id = Id.Value;

            return writer;
        }
    }
}