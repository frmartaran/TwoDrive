using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private Claim read;
        private Claim write;
        private Claim share;

        [TestInitialize]
        public void SetUp()
        {

            root = new Folder
            {
                Name = "Root"
            };
            read = new Claim
            {
                Element = root,
                Type = ClaimType.Read
            };
            write = new Claim
            {
                Element = root,
                Type = ClaimType.Write
            };
            share = new Claim
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
            var canDelete = writer.HasClaimsFor(root, ClaimType.Delete);

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
            var delete = new Claim
            {
                Element = root,
                Type = ClaimType.Delete
            };

            Assert.IsTrue(writer.Claims.Contains(read));
            Assert.IsTrue(writer.Claims.Contains(write));
            Assert.IsTrue(writer.Claims.Contains(share));
            Assert.IsFalse(writer.Claims.Contains(delete));

        }
    }
}