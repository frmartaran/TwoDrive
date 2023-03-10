
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Exceptions;
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
            var writer = new Writer
            {
                Id = 1,
                UserName = "Username",
                Password = "Password",
                Friends = new List<Writer>(),
            };
            var mockRepository = new Mock<IRepository<Session>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Insert(It.IsAny<Session>()));
            mockRepository.Setup(m => m.Save());
            var testList = new List<Writer>{
                writer
            };
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

        [TestMethod]
        public void CreateSessionUserDoesntExists()
        {
            var context = ContextFactory.GetMemoryContext("Create Session Null Case");
            var repository = new SessionRepository(context);
            var writerRepository = new WriterRepository(context);

            var logic = new SessionLogic(repository, writerRepository);
            var username = "Username";
            var password = "Password";
            logic.Create(username, password);
            var sessionInContext = context.Sessions
                                    .Include(s => s.Writer)
                                    .FirstOrDefault();
            Assert.IsNull(sessionInContext);
        }

        [TestMethod]
        public void GetWriterByTokenMock()
        {
            var mockRepository = new Mock<IRepository<Session>>(MockBehavior.Strict);
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var testList = new List<Session>();
            mockRepository.Setup(m => m.GetAll())
                            .Returns(testList);
            var logic = new SessionLogic(mockRepository.Object, mockWriterRepository.Object);
            Writer writer = logic.GetWriter(Guid.NewGuid().ToString());
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void GetWriterByToken()
        {
            var context = ContextFactory.GetMemoryContext("Get Writer By Token");
            var repository = new SessionRepository(context);
            var mockWriterRepository = new Mock<IRepository<Writer>>();

            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var writer = new Writer
            {
                Id = 1,
                UserName = "Username",
                Password = "Password",
                Friends = new List<Writer>(),
            };
            var token = Guid.NewGuid();
            var session = new Session
            {
                Writer = writer,
                Token = token
            };
            context.Sessions.Add(session);
            context.SaveChanges();

            var writerInDb = logic.GetWriter(token.ToString());
            Assert.AreEqual(writer, writerInDb);
        }

        [TestMethod]
        public void NoWriterWithToken()
        {
            var context = ContextFactory.GetMemoryContext("No Writer With Token");
            var repository = new SessionRepository(context);
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var token = Guid.NewGuid();

            var writerInDb = logic.GetWriter(token.ToString());
            Assert.IsNull(writerInDb);
        }

        [TestMethod]
        public void HasLevel()
        {
            var context = ContextFactory.GetMemoryContext("Has level test");
            var repository = new SessionRepository(context);
            var writer = new Writer
            {
                Id = 1,
                UserName = "Username",
                Password = "Password",
                Role = Role.Administrator,
                Friends = new List<Writer>(),
            };
            var token = Guid.NewGuid();
            var session = new Session
            {
                Writer = writer,
                Token = token
            };
            context.Sessions.Add(session);
            context.SaveChanges();
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var hasLevel = logic.HasLevel(token.ToString());
            Assert.IsTrue(hasLevel);

        }

        [TestMethod]
        public void DoesntHaveLevel()
        {
            var context = ContextFactory.GetMemoryContext("Doesn't have test");
            var repository = new SessionRepository(context);
            var writer = new Writer
            {
                Id = 1,
                UserName = "Username",
                Password = "Password",
                Role = Role.Writer,
                Friends = new List<Writer>(),
            };
            var token = Guid.NewGuid();
            var session = new Session
            {
                Writer = writer,
                Token = token
            };
            context.Sessions.Add(session);
            context.SaveChanges();
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var hasLevel = logic.HasLevel(token.ToString());
            Assert.IsFalse(hasLevel);

        }

        [TestMethod]
        public void ThereIsNoSession()
        {
            var context = ContextFactory.GetMemoryContext("Doesn't have test");
            var repository = new SessionRepository(context);
            var token = Guid.NewGuid();
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var hasLevel = logic.HasLevel(token.ToString());
            Assert.IsFalse(hasLevel);
        }

        [TestMethod]
        public void IsValidTokenMock()
        {
            var mockRepository = new Mock<IRepository<Session>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Exists(It.IsAny<Session>()))
            .Returns(true);
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(mockRepository.Object, mockWriterRepository.Object);
            logic.IsValidToken(Guid.NewGuid().ToString());
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void IsNotValidTokenMock()
        {
            var mockRepository = new Mock<IRepository<Session>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Exists(It.IsAny<Session>()))
            .Returns(false);
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(mockRepository.Object, mockWriterRepository.Object);
            logic.IsValidToken(Guid.NewGuid().ToString());
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void IsValidToken()
        {
            var context = ContextFactory.GetMemoryContext("Is valid");
            var token = Guid.NewGuid();
            var session = new Session
            {
                Writer = new Writer(),
                Token = token
            };
            var repository = new SessionRepository(context);
            repository.Insert(session);
            repository.Save();
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var valid = logic.IsValidToken(token.ToString());
            Assert.IsTrue(valid);

        }

        [TestMethod]
        public void IsNotValidToken()
        {
            var context = ContextFactory.GetMemoryContext("Is valid");
            var token = Guid.NewGuid();
            var repository = new SessionRepository(context);
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var valid = logic.IsValidToken(token.ToString());
            Assert.IsFalse(valid);

        }

        [TestMethod]
        public void InvalidStringToken()
        {
            var context = ContextFactory.GetMemoryContext("Is valid");
            var token = Guid.NewGuid();
            var repository = new SessionRepository(context);
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var valid = logic.IsValidToken("InvalidToken");
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void RemoveSession()
        {
            var context = ContextFactory.GetMemoryContext("remove session");
            var repository = new SessionRepository(context);
            var session = new Session
            {
                Id = 1,
                Token = Guid.NewGuid(),
                Writer = new Writer()
            };
            repository.Insert(session);
            repository.Save();

            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            logic.RemoveSession(session);

            var allSession = repository.GetAll();
            Assert.AreEqual(0, allSession.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void RemoveNullSession()
        {
            var context = ContextFactory.GetMemoryContext("remove null session");
            var repository = new SessionRepository(context);

            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            logic.RemoveSession(null);
        }

        [TestMethod]
        public void GetCurrentSession()
        {
            var context = ContextFactory.GetMemoryContext("get session");
            var repository = new SessionRepository(context);
            var token = Guid.NewGuid();
            var session = new Session
            {
                Id = 1,
                Token = token,
                Writer = new Writer()
            };
            repository.Insert(session);
            repository.Save();
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var current = logic.GetSession(token.ToString());

            Assert.AreEqual(token, current.Token);
            Assert.AreEqual(session.Writer, current.Writer);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void GetNullSession()
        {
            var context = ContextFactory.GetMemoryContext("get session");
            var repository = new SessionRepository(context);
            var mockWriterRepository = new Mock<IRepository<Writer>>();
            var logic = new SessionLogic(repository, mockWriterRepository.Object);
            var current = logic.GetSession(null);
        }
    }
}