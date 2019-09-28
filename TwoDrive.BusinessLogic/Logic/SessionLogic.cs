
using System;
using System.Linq;
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

        public SessionLogic(IRepository<Session> repository)
        {
            Repository = repository;
        }

        public Guid? Create(string username, string password)
        {
            var user = WriterRepository.GetAll()
                        .Where(w => w.UserName == username)
                        .Where(w => w.Password == password)
                        .FirstOrDefault();
            if (user == null)
                return null;
            var token = Guid.NewGuid();
            var session = new Session
            {
                Writer = user,
                Token = token
            };
            Repository.Insert(session);
            Repository.Save();
            return token;
        }

        public Writer GetWriter(Guid token)
        {
            var writer = Repository.GetAll()
                    .Where(s => s.Token == token)
                    .Select(s => s.Writer)
                    .FirstOrDefault();
            return writer;
        }

        public bool HasLevel(Guid token)
        {
            var writer = GetWriter(token);
            if(writer == null)
                return false;
            return writer.Role == Role.Administrator;
        }
    }
}