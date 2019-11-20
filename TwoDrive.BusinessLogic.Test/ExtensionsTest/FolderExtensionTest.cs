using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test.ExtensionsTest
{
    [TestClass]
    public class FolderExtensionTest
    {
        private Writer writer;
        private Folder root;
        private Folder child;

        [TestInitialize]
        public void SetUp()
        {
            root = new Folder
            {
                Name = "Root"
            };
            var read = new CustomClaim
            {
                Element = root,
                Type = ClaimType.Read
            };
            var defaultClaims = new List<CustomClaim>{
                read,
            };
            writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<WriterFriend>(),
                Claims = defaultClaims,
            };
            root.Owner = writer;
            child = new Folder
            {
                Id = 2,
                Name = "First Child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var readChild = new CustomClaim
            {
                Element = child,
                Type = ClaimType.Read
            };
            writer.Claims.Add(readChild);
        }

        [TestMethod]
        public void WriterHasClaimsForParent()
        {
            var result = FolderLogicExtension.WriterHasClaimsForParent(writer.Claims, child, child.Id);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WriterHasNoClaimsForParent()
        {
            writer.Claims = writer.Claims
                .Where(c => c.Element.Name != "Root")
                .ToList();
            var result = FolderLogicExtension.WriterHasClaimsForParent(writer.Claims, child, child.Id);
            Assert.IsFalse(result);
        }
    }
}
