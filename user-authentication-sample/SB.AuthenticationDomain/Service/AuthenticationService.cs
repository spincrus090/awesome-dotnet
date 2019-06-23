using SB.AuthenticationDomain.Repository.Interface;
using SB.AuthenticationDomain.Service.Interface;
using SB.Helper;
using SB.Model.Entity.Authentication;
using System;
using System.Collections.Generic;

namespace SB.AuthenticationDomain.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfilePhotoRepository _profilePhotoRepository;

        public AuthenticationService(IUserRepository userRepository, IProfilePhotoRepository profilePhotoRepository)
        {
            _userRepository = userRepository;
            _profilePhotoRepository = profilePhotoRepository;
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }                

            User user = _userRepository.GetByUserName(username);

            // check if username exists
            if (user == null)
            {
                return null;
            }                

            // check if password is correct
            if (!SecurityHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }                

            // authentication successful
            return user;
        }

        public User Register(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password is required");
            }

            if (_userRepository.GetByUserName(user.Username) != null)
            {
                throw new Exception("Username '" + user.Username + "' is already taken");
            }

            byte[] passwordHash, passwordSalt;
            SecurityHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return InsertUser(user);
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetAll();
        }

        public User GetById(Guid id)
        {
            return _userRepository.Get(id);
        }

        public void UpdateUser(User userParam, string password)
        {
            var user = _userRepository.Get(userParam.Id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_userRepository.GetByUserName(userParam.Username) != null)
                {
                    throw new Exception("Username " + userParam.Username + " is already taken");
                }
            }

            // update user properties
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                SecurityHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _userRepository.Update(user);
        }

        public void DeleteUser(Guid id)
        {
            var user = _userRepository.Get(id);

            if (user != null)
            {
                _userRepository.Delete(user);
            }
        }

        public User InsertUser(User user)
        {
            return _userRepository.Insert(user);
        }
    }
}
