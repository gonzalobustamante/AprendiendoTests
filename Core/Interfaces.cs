using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        void AddUser(User user);
        void AddRoleToUser(int userId, Role role);
    }

    public interface IRoleRepository
    {
        Role GetRoleByName(string roleName);
        List<Role> GetAllRoles();
    }

    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);//esta hecho
        void Register(string username, string password);
        void AssignRoleToUser(string username, string roleName);
    }

}
