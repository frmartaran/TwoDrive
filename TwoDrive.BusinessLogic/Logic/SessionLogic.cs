
using System;
using System.Linq;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Logic
{
    public class SessionLogic
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
    }
}