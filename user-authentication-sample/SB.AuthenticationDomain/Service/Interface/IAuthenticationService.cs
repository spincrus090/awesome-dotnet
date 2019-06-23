using SB.Model.Entity.Authentication;
using System;
using System.Collections.Generic;

namespace SB.AuthenticationDomain.Service.Interface
{
    public interface IAuthenticationService
    {
        User InsertUser(User user);
        User Login(string username, string password);
        User Register(User user, string password);
        IEnumerable<User> GetUsers();
        User GetById(Guid id);
        void UpdateUser(User user, string password);
        void DeleteUser(Guid id);
    }
}
