using System;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic
{
    public interface ISessionLogic
    {
        Guid? Create(string username, string password);

        Writer GetWriter(string token);

        bool HasLevel(string token);

        bool IsValidToken(string token);


    }
}