
using System;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Logic
{
    public class SessionLogic : ISessionLogic
    {
        private IRepository<Session> Repository { get; set; }
        private IRepository<Writer> WriterRepository { get; set; }
        public SessionLogic(IRepository<Session> repository, IRepository<Writer> writerRepository)
        {
            Repository = repository;
            WriterRepository = writerRepository;
        }

        public Guid? Create(string username, string password)
        {
            var user = FetchWriter(username, password);

            if (user == null)
                return null;

            var token = Guid.NewGuid();
            var session = new Session
            {
                Writer = user,
                Token = token
            };
            SaveSession(session);
            return token;
        }

        private void SaveSession(Session session)
        {
            Repository.Insert(session);
            Repository.Save();
        }

        private Writer FetchWriter(string username, string password)
        {
            return WriterRepository.GetAll()
                        .Where(w => w.UserName == username)
                        .Where(w => w.Password == password)
                        .FirstOrDefault();
        }

        public Writer GetWriter(string token)
        {
            var writer = Repository.GetAll()
                    .Where(s => s.Token.ToString() == token)
                    .Select(s => s.Writer)
                    .FirstOrDefault();
            return writer;
        }

        public bool HasLevel(string token)
        {
            var writer = GetWriter(token);
            if (writer == null)
                return false;
            return writer.Role == Role.Administrator;
        }

        public bool IsValidToken(string token)
        {
            Guid realToken;

            if (!Guid.TryParse(token, out realToken))
                return false;

            var auxSession = new Session
            {
                Token = realToken
            };
            return Repository.Exists(auxSession);
        }

        public void RemoveSession(Session session)
        {
            try
            {
                Repository.Delete(session.Id);
                Repository.Save();
            }
            catch (NullReferenceException exception)
            {
                throw new LogicException(exception.Message, exception);
            }
        }

        public Session GetSession(string token)
        {
            try
            {
                return Repository.GetAll()
                     .Where(s => s.Token.ToString() == token)
                     .First();
            }
            catch (InvalidOperationException exception)
            {
                throw new LogicException(exception.Message, exception);
            }
        }
    }
}