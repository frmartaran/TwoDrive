using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class WriterLogicTest
    {
        private Writer writer;
        [TestInitialize]
        public void SetUp()
        {
            var root = new Folder
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
                Token = Guid.NewGuid(),
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };
        }
        [TestMethod]
        public void CreateUser()
        {
            var mockRepository = new Mock<IRepository<Writer>>(MockBehavior.Strict);
            mockRepository
            .Setup(m => m.Insert(It.IsAny<Writer>()));
            mockRepository.Setup(m => m.Save());

            var mockValidator = new Mock<IValidator<Writer>>(MockBehavior.Strict);
            mockValidator
            .Setup(m => m.isValid(It.IsAny<Writer>()));

            var logic = new WriterLogic(mockRepository.Object, mockValidator.Object);
            logic.Create(writer);

            mockRepository.VerifyAll();

        }
    }
}