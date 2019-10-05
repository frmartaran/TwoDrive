using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class WriterExtensionTest
    {
        private Writer writer;
        private Folder root;

        [TestInitialize]
        public void SetUp()
        {

            root = new Folder
            {
                Name = "Root"
            };
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
            var defaultClaims = new List<Claim>{
                write,
                read,
                share
            };
            writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };
            root.Owner = writer;
        }

        [TestMethod]
        public void WriterHasClaimsFor()
        {
            var canWrite = writer.HasClaimsFor(root, ClaimType.Write);
            var canRead = writer.HasClaimsFor(root, ClaimType.Read);
            var canShare = writer.HasClaimsFor(root, ClaimType.Share);

            Assert.IsTrue(canWrite);
            Assert.IsTrue(canRead);
            Assert.IsTrue(canShare);

        }


        [TestMethod]
        public void WriterDoesntHaveClaimsFor()
        {
            var file = new TxtFile();
            var canWrite = writer.HasClaimsFor(file, ClaimType.Write);
            var canRead = writer.HasClaimsFor(file, ClaimType.Read);
            var canShare = writer.HasClaimsFor(file, ClaimType.Share);
            var canDelete = writer.HasClaimsFor(file, ClaimType.Delete);

            Assert.IsFalse(canWrite);
            Assert.IsFalse(canRead);
            Assert.IsFalse(canShare);
            Assert.IsFalse(canDelete);

        }

        [TestMethod]
        public void IsFriendsWith()
        {
            var friend = new Writer();
            writer.Friends.Add(friend);
            var isAFriend = writer.IsFriendsWith(friend);
            Assert.IsTrue(isAFriend);
        }


        [TestMethod]
        public void IsNotFriendsWith()
        {
            var friend = new Writer();
            var isAFriend = writer.IsFriendsWith(friend);
            Assert.IsFalse(isAFriend);
        }

        [TestMethod]
        public void AddRootClaims()
        {
            writer.Claims = new List<Claim>();
            writer.AddRootClaims(root);

            Assert.IsTrue(writer.Claims.Any(c => c.Type == ClaimType.Read));
            Assert.IsTrue(writer.Claims.Any(c => c.Type == ClaimType.Write));
            Assert.IsTrue(writer.Claims.Any(c => c.Type == ClaimType.Share));
            Assert.IsFalse(writer.Claims.Any(c => c.Type == ClaimType.Delete));

        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void AddRootClaimsToNonRoot()
        {
            var folder = new Folder
            {
                Owner = writer,
                Name = "Folder",
                ParentFolder = root
            };
            writer.Claims = new List<Claim>();
            writer.AddRootClaims(folder);
        }

        [TestMethod]
        public void AddCreatorClaimsTo()
        {
            writer.Claims = new List<Claim>();
            var folder = new Folder
            {
                Name = "Folder",
                Owner = writer,
                ParentFolder = root
            };
            writer.AddCreatorClaimsTo(folder);

            Assert.IsTrue(writer.Claims.Any(c => c.Type == ClaimType.Read));
            Assert.IsTrue(writer.Claims.Any(c => c.Type == ClaimType.Write));
            Assert.IsTrue(writer.Claims.Any(c => c.Type == ClaimType.Share));
            Assert.IsTrue(writer.Claims.Any(c => c.Type == ClaimType.Delete));
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void AddCreatorClaimsToOtherWriter()
        {
            writer.Claims = new List<Claim>();
            var folder = new Folder
            {
                Name = "Folder",
                Owner = new Writer(),
                ParentFolder = root
            };
            writer.AddCreatorClaimsTo(folder);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void AddCreatorClaimsToRootFolder()
        {
            writer.Claims = new List<Claim>();
            writer.AddCreatorClaimsTo(root);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void AlreadyHasClaims()
        {
            var folder = new Folder
            {
                Owner = writer,
                Name = "Folder",
                ParentFolder = root
            };
            var read = new Claim
            {
                Element = folder,
                Type = ClaimType.Read
            };
            var write = new Claim
            {
                Element = folder,
                Type = ClaimType.Write
            };
            var share = new Claim
            {
                Element = folder,
                Type = ClaimType.Share
            };
            var delete = new Claim
            {
                Element = folder,
                Type = ClaimType.Delete
            };
            var defaultClaims = new List<Claim>{
                write,
                read,
                share,
                delete
            };
            writer.Claims.Add(read);
            writer.Claims.Add(write);
            writer.Claims.Add(share);
            writer.Claims.Add(delete);
            writer.AddCreatorClaimsTo(folder);
        }


        [TestMethod]
        public void RemoveClaim()
        {
            writer.Claims = new List<Claim>();
            var folder = new Folder
            {
                Owner = writer,
                Name = "Folder",
                ParentFolder = root
            };
            var read = new Claim
            {
                Element = folder,
                Type = ClaimType.Read
            };
            var write = new Claim
            {
                Element = folder,
                Type = ClaimType.Write
            };
            var share = new Claim
            {
                Element = folder,
                Type = ClaimType.Share
            };
            var delete = new Claim
            {
                Element = folder,
                Type = ClaimType.Delete
            };
            var defaultClaims = new List<Claim>{
                write,
                read,
                share,
                delete
            };
            writer.AddCreatorClaimsTo(folder);
            writer.RemoveAllClaims(folder);
            Assert.AreEqual(0, writer.Claims.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void RemoveAllNonExistantClaims()
        {
            writer.Claims = new List<Claim>();
            var folder = new Folder
            {
                Owner = writer,
                Name = "Folder",
                ParentFolder = root
            };
            writer.RemoveAllClaims(folder);
        }

        [TestMethod]
        public void AllowFriendToRead()
        {
            var friend = new Writer
            {
                Claims = new List<Claim>()
            };
            writer.Friends.Add(friend);
            writer.AllowFriendTo(friend, root, ClaimType.Read);
            var canFriendRead = friend.Claims
                .Where(c => c.Element == root)
                .FirstOrDefault();
            Assert.AreEqual(ClaimType.Read, canFriendRead.Type);

        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void DontAllowStrangerToRead()
        {
            var friend = new Writer
            {
                Claims = new List<Claim>()
            };
            writer.AllowFriendTo(friend, root, ClaimType.Read);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void IsAlraedyAllowToRead()
        {
            var friend = new Writer
            {
                Claims = new List<Claim>()
            };
            writer.Friends.Add(friend);
            writer.AllowFriendTo(friend, root, ClaimType.Read);
            writer.AllowFriendTo(friend, root, ClaimType.Read);
        }

        [TestMethod]
        public void RevokeFriendFrom()
        {
            var friend = new Writer
            {
                Claims = new List<Claim>()
            };
            writer.Friends.Add(friend);
            writer.AllowFriendTo(friend, root, ClaimType.Read);
            writer.RevokeFriendFrom(friend, root, ClaimType.Read);

            Assert.AreEqual(0, friend.Claims.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void RevokeFriendFromTheirElement()
        {
            var folder = root;
            var friend = new Writer
            {
                Claims = new List<Claim>()
            };
            var read = new Claim
            {
                Element = folder,
                Type = ClaimType.Read
            };
            friend.Claims.Add(read);
            folder.Owner = friend;
            writer.Friends.Add(friend);
            writer.RevokeFriendFrom(friend, root, ClaimType.Read);

        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void RevokeFriendFromNoClaims()
        {
            var friend = new Writer
            {
                Claims = new List<Claim>()
            };
            writer.Friends.Add(friend);
            writer.RevokeFriendFrom(friend, root, ClaimType.Read);
        }
    }
}