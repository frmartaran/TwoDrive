
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Helpers;
using TwoDrive.BusinessLogic.Resources;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Extensions
{
    public static class WriterLogicExtension
    {
        public static bool HasClaimsFor(this Writer writer, Element element, ClaimType type)
        {
            return writer.Claims
                    .Where(c => c.Element == element)
                    .Where(c => c.Type == type)
                    .Any();
        }

        public static bool IsFriendsWith(this Writer writer, Writer friend)
        {
            return writer.Friends.Any(f => f.FriendId == friend.Id);
        }

        public static bool AreFriendAndWriterTheSameWriter(this Writer writer, Writer friend)
        {
            return friend.Id == writer.Id;
        }

        public static void AddRootClaims(this Writer writer, Folder root)
        {
            ValidateIsRoot(root);
            var claims = CreateBasicClaims(root);
            writer.Claims.AddRange(claims);
        }

        private static void ValidateIsRoot(Folder root)
        {
            if (!IsRoot(root))
                throw new LogicException(BusinessResource.RootClaims_Claims);
        }

        private static bool IsRoot(Element root)
        {
            return root.ParentFolder == null && root.Name == "Root";
        }

        public static void AddCreatorClaimsTo(this Writer writer, Element element)
        {
            ValidateIfOwner(writer, element);
            ValidateIsNotRoot(element);
            writer.AlreadyHasClaimsFor(element);
            var claims = CreateBasicClaims(element);
            var delete = new CustomClaim
            {
                Element = element,
                Type = ClaimType.Delete
            };
            writer.Claims.Add(delete);
            writer.Claims.AddRange(claims);
        }

        private static List<CustomClaim> CreateBasicClaims(Element element)
        {
            var read = new CustomClaim
            {
                Element = element,
                Type = ClaimType.Read
            };
            var write = new CustomClaim
            {
                Element = element,
                Type = ClaimType.Write
            };
            var share = new CustomClaim
            {
                Element = element,
                Type = ClaimType.Share
            };
            return new List<CustomClaim>
            {
                read,
                write,
                share
            };
        }

        private static void AlreadyHasClaimsFor(this Writer writer, Element element)
        {
            var canAlreadyRead = writer.HasClaimsFor(element, ClaimType.Read);
            var canAlreadyWrite = writer.HasClaimsFor(element, ClaimType.Write);
            var canAlreadyShare = writer.HasClaimsFor(element, ClaimType.Share);
            var canAlreadyDelete = writer.HasClaimsFor(element, ClaimType.Delete);

            if (canAlreadyRead && canAlreadyWrite && canAlreadyShare && canAlreadyDelete)
                throw new LogicException(BusinessResource.ExisitngCreatorClaims_Claims);
        }

        private static void ValidateIsNotRoot(Element element)
        {
            if (IsRoot(element))
                throw new LogicException(BusinessResource.CantAddCreatorClaims);
        }

        private static void ValidateIfOwner(Writer writer, Element element)
        {
            if (element.Owner != writer)
                throw new LogicException(string.Format(BusinessResource.NotOwner, writer.UserName));
        }

        public static void RemoveAllClaims(this Writer writer, Element element)
        {
            var allClaims = writer.Claims
                .Where(c => c.Element == element)
                .ToList();

            if (allClaims.Count == 0)
                throw new LogicException(BusinessResource.NoClaims);

            writer.Claims.RemoveRange(allClaims);
        }

        public static void AllowFriendTo(this Writer owner, Writer friend, Element element, ClaimType type)
        {
            ValidateIfFriends(owner, friend);
            ValidateIfFriendCan(friend, element, type);

            var claim = new CustomClaim
            {
                Element = element,
                Type = type
            };
            friend.Claims.Add(claim);

        }

        private static void ValidateIfFriends(Writer owner, Writer friend)
        {
            var areFriends = owner.IsFriendsWith(friend);
            if (!areFriends)
                throw new LogicException(string.Format(BusinessResource.NotFriends, friend.UserName));
        }

        private static void ValidateIfFriendCan(Writer friend, Element element, ClaimType type)
        {
            var canAlreardy = friend.HasClaimsFor(element, type);
            if (canAlreardy)
                throw new LogicException(string.Format(BusinessResource.AlreadyHas_Claims,
                    friend.UserName, type.ToString()));
        }

        public static void RevokeFriendFrom(this Writer writer, Writer friend, Element element, ClaimType type)
        {
            ValidateIfOwner(writer, element);

            var claim = friend.Claims
                .Where(e => e.Element == element)
                .Where(c => c.Type == type)
                .FirstOrDefault();

            ValidateIfFriendCan(claim);

            friend.Claims.Remove(claim);
        }

        private static void ValidateIfFriendCan(CustomClaim claim)
        {
            if (claim == null)
                throw new LogicException(BusinessResource.NoClaims);
        }
    }
}