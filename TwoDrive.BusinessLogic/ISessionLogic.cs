using System;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic
{
    public interface ISessionLogic
    {
        Guid? Create(string username, string password);

        Writer GetWriter(Guid token);

        bool HasLevel(Guid token);


    }
}