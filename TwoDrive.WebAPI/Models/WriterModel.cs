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

            if (Friends != null)
            {
                Friends = entity.Friends
                .Select(e => new WriterModel().FromDomain(e.Friend))
                .ToList();
            }
            if (Claims != null)
            {
                Claims = entity.Claims
                .Select(c => new ClaimModel().FromDomain(c))
                .ToList();
            }

            return this;
        }

        public Writer ToDomain(Writer writer)
        {
            if (this == null)
                return null;
            if (writer == null)
                return null;

            writer.Role = this.Role;
            writer.UserName = this.UserName;
            writer.Password = this.Password;
            writer.Friends = this.Friends?
                        .Select(f => new WriterFriend 
                        {
                            Writer = writer,
                            Friend = f.ToDomain()
                        })
                        .ToList() ?? null;
            writer.Claims = this.Claims?
                    .Select(c => c.ToDomain())
                    .ToList() ?? null;

            if (Id.HasValue)
                writer.Id = Id.Value;

            return writer;
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
                Claims = this.Claims?
                    .Select(c => c.ToDomain())
                    .ToList() ?? new List<CustomClaim>()
            };
            writer.Friends = this.Friends?
                        .Select(f => new WriterFriend
                        {
                            Writer = writer,
                            Friend = f.ToDomain()
                        })
                        .ToList() ?? new List<WriterFriend>();
            if (Id.HasValue)
                writer.Id = Id.Value;

            return writer;
        }
    }
}