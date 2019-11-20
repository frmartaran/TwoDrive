using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.WebApi.Models
{
    public class WriterModel
    {
        public int? Id { get; set; }

        public Role Role { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICollection<WriterModel> Friends { get; set; }

        public ICollection<ClaimModel> Claims { get; set; }

        public WriterModel FromDomain(Writer entity, bool isFirstLevel = true)
        {
            if (entity == null)
                return null;

            Id = entity.Id;
            Role = entity.Role;
            UserName = entity.UserName;

            if (entity.Friends != null && isFirstLevel)
            {
                Friends = entity.Friends
                .Select(wf => new WriterModel().FromDomain(wf.Friend, false))
                .ToList();
            }
            if (entity.Claims != null)
            {
                Claims = new List<ClaimModel>();
                var claimsByElements = entity.Claims
                .GroupBy(c => c.ElementId)
                .ToList();
                foreach (var claimGrouping in claimsByElements)
                {
                    var claimElement = claimGrouping
                    .Select(c => c)
                    .FirstOrDefault()
                    .Element;
                    if (FolderLogicExtension.ElementIsFromOwnerAndIsRootFolder(claimElement, Id.Value) 
                        || FolderLogicExtension.ElementIsntFromOwner(claimElement, Id.Value) 
                        || FolderLogicExtension.WriterHasClaimsForParent(entity.Claims, claimElement, claimElement.Id))
                    {
                        Claims.Add(new ClaimModel().FromDomain(claimGrouping));
                    }                    
                }
            }

            return this;
        }

        /// <summary>
        /// This method merges information coming from the frontend and backend. Doesn't update claims or friends.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public Writer ToDomain(Writer writer)
        {
            if (this == null)
                return null;
            if (writer == null)
                return null;

            writer.Role = this.Role;
            writer.UserName = this.UserName;
            writer.Password = this.Password;

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
                Claims = GetModelClaims()
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

        public ICollection<CustomClaim> GetModelClaims()
        {
            var result = new List<CustomClaim>();
            if (Claims != null)
            {
                foreach (var claim in Claims)
                {
                    result.AddRange(claim.ToDomain());
                }
            }
            return result;
        }
    }
}