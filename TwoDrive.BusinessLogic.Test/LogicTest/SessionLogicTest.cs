
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            var logic = new SessionLogic(mockRepository.Object);
            logic.Create("Username", "Password");
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void CreateSession()
        {
            var context = ContextFactory.GetMemoryContext("Create Session");
            var logic = new SessionLogic(context);
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