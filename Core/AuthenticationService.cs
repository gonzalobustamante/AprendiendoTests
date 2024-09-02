using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthenticationService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public bool Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            return user != null && user.Password == password;
        }

        public void Register(string username, string password)
        {
            if (_userRepository.GetUserByUsername(username) != null)
            {
                throw new ArgumentException("Username already exists");
            }

            var user = new User
            {
                Username = username,
                Password = password
            };
            _userRepository.AddUser(user);
        }

        public void AssignRoleToUser(string username, string roleName)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }

            var role = _roleRepository.GetRoleByName(roleName);
            if (role == null)
            {
                throw new ArgumentException("Role does not exist");
            }

            _userRepository.AddRoleToUser(user.Id, role);
        }
    }
}
