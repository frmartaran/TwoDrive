
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class SessionLogicTest
    {
        [TestMethod]
        public void CreateSessionMock()
        {
            var mockRepository = new Mock<IRepository<Session>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Insert(It.IsAny<Session>()));
            mockRepository.Setup(m => m.Save());
            var testList = new List<Writer>();
            var mockWriterRepository = new Mock<IRepository<Writer>>(MockBehavior.Strict);
            mockWriterRepository.Setup(m => m.GetAll())
                                .Returns(testList);

            var logic = new SessionLogic(mockRepository.Object, mockWriterRepository.Object);
            logic.Create("Username", "Password");
            mockRepository.VerifyAll();
            mockWriterRepository.VerifyAll();
        }

        [TestMethod]
        public void CreateSession()
        {
            var context = ContextFactory.GetMemoryContext("Create Session");
            var repository = new SessionRepository(context);
            var writerRepository = new WriterRepository(context);
            var writer = new Writer
            {
                Id = 1,
                UserName = "Username",
                Password = "Password",
                Friends = new List<Writer>(),
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var logic = new SessionLogic(repository, writerRepository);
            var username = "Username";
            var password = "Password";
            logic.Create(username, password);
            var sessionInContext = context.Sessions
                                    .Include(s => s.Writer)
                                    .FirstOrDefault();
            Assert.AreEqual(username, sessionInContext.Writer.UserName);
            Assert.AreEqual(password, sessionInContext.Writer.Password);
        }
    }
}