
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
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
            return writer.Friends.Contains(friend);
        }

        public static void AddRootClaims(this Writer writer, Folder root)
        {
            bool isRoot = IsRoot(root);
            if (!isRoot)
                throw new LogicException("Can't add root claims to a child folder");

            var read = new Claim
            {
                Element = root,
                Type = ClaimType.Read
            };
            var write = new Claim
            {
                Element = root,
                Type = ClaimType.Write
            };
            var share = new Claim
            {
                Element = root,
                Type = ClaimType.Share
            };
            writer.Claims.Add(read);
            writer.Claims.Add(write);
            writer.Claims.Add(share);
        }

        private static bool IsRoot(Element root)
        {
            return root.ParentFolder == null && root.Name == "Root";
        }

        public static void AddCreatorClaimsTo(this Writer writer, Element element)
        {
            if (element.Owner != writer)
                throw new LogicException("Can only add creator claims to it's owner");
            if (IsRoot(element))
                throw new LogicException("Can't add creator claims to root");


            var read = new Claim
            {
                Element = element,
                Type = ClaimType.Read
            };
            var write = new Claim
            {
                Element = element,
                Type = ClaimType.Write
            };
            var share = new Claim
            {
                Element = element,
                Type = ClaimType.Share
            };
            var delete = new Claim
            {
                Element = element,
                Type = ClaimType.Delete
            };
            writer.Claims.Add(read);
            writer.Claims.Add(write);
            writer.Claims.Add(share);
            writer.Claims.Add(delete);
        }
    }
}