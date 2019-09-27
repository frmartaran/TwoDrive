using System;
namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface ISessionLogic<UserType>
    where UserType: class
    {
        Guid? Create(string username, string password);

        UserType GetWriter(Guid token);

        bool HasLevel(Guid token);


    }
}