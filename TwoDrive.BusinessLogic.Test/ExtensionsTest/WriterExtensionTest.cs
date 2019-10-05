using System.Collections.Generic;
using System.Linq;
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
    }
}