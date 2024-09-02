using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            user.Id = _users.Count + 1;
            _users.Add(user);
        }

        public void AddRoleToUser(int userId, Role role)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Roles.Add(role);
            }
        }       
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly List<Role> _roles = new List<Role>
        {
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        };

        public Role GetRoleByName(string roleName)
        {
            return _roles.FirstOrDefault(r => r.Name == roleName);
        }

        public List<Role> GetAllRoles()
        {
            return _roles;
        }
    }

}
