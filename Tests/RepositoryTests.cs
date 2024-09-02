using Core;

namespace Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private IRoleRepository rolRepository;
        private IUserRepository userRepository;
        private IAuthenticationService authenticationService;

        [TestInitialize]
        public void Setup()
        {
            rolRepository = new RoleRepository();
            userRepository = new UserRepository();
            authenticationService = new AuthenticationService(userRepository, rolRepository); // Inicializa AuthenticationService antes de cada test
        }

        [TestMethod]
        public void GetRoleByName_ShouldReturnOK()
        {
            //Arrange
            string roleName = "Admin";
            //Act
            var result = rolRepository.GetRoleByName(roleName);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roleName, result.Name);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void GetRolByName_WhenParametersAreIncorrect_ShouldReturnNull()
        {
            //Arrange
            string roleName = "INEXISTENTE";
            //Act
            var result = rolRepository.GetRoleByName(roleName);
            //Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public void GetAllRoles_ShouldReturnList()
        {
            //Act
            var result = rolRepository.GetAllRoles();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        /*Tests method for Users*/
        [TestMethod]
        public void AddUser_ShouldAddUser()
        {
            //Arrange
            var user = new User { Username = "admin", Password = "admin" };
            //Act
            userRepository.AddUser(user);
            var result = userRepository.GetUserByUsername("admin");
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("admin", result.Username);
        }
        [TestMethod]
        public void AddUser_WhenUserAlreadyExists()
        {
            //Arrange
            User user = new User { Username = "lpinedo", Password = "password" };
            User existingUser = new User { Username = "lpinedo", Password = "asdasdas" };
            //Act
            userRepository.AddUser(user);
            var result = userRepository.GetUserByUsername("lpinedo");
            //Assert
            Assert.AreEqual(existingUser.Username, result.Username, "Ya existe el usuario");
        }

        [TestMethod]
        public void GetUserByUsername_ShouldReturnUser()
        {
            //Arrange
            var user = new User { Username = "lpinedo", Password = "password" };
            var username = "lpinedo";

            //Act
            userRepository.AddUser(user);
            var result = userRepository.GetUserByUsername(username);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("lpinedo", result.Username);
        }
        [TestMethod]
        public void GetUserByUsername_WhenUserDoesNotExist_ShouldReturnNull()
        {
            //Arrange
            var username = "asd";
            //Act
            var result = userRepository.GetUserByUsername(username);
            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddRoleToUser_ShouldAddRole()
        {
            /*//Arrange
            var user = new User { Username = "lpinedo", Password = "password" };
            var role = new Role { Id = 3, Name = "asdasd" };
            bool RoleExistsInUser = false;
            //Act
            userRepository.AddUser(user);
            var result = userRepository.GetUserByUsername("lpinedo");
            userRepository.AddRoleToUser(result.Id, role);
            var userWithRole = userRepository.GetUserByUsername("lpinedo");

            foreach (var item in userWithRole.Roles)
            {
                if (item.Name == role.Name)
                {
                    RoleExistsInUser = true;
                }
            }
            //Assert
            Assert.AreEqual(true, RoleExistsInUser);*/
            //Arrange
            var user = new User { Username = "lpinedo", Password = "password" };
            var role = new Role { Id = 3, Name = "Jefe" };
            //Act
            userRepository.AddUser(user);
            var result = userRepository.GetUserByUsername("lpinedo");
            userRepository.AddRoleToUser(result.Id, role);
            var userWithRole = userRepository.GetUserByUsername("lpinedo");
            //Assert
            Assert.IsNotNull(userWithRole);
            Assert.AreEqual(1, userWithRole.Roles.Count);
        }

        /*Tests AuthenticationService*/
        [TestMethod]
        public void Authenticate_ShouldReturnTrue()
        {
            //Arrange
            var user = new User { Username = "lpinedo", Password = "password" };
            var username = "lpinedo";
            var password = "password";
            //Act
            userRepository.AddUser(user);
            var result = authenticationService.Authenticate(username, password);
            //Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void Authenticate_ShouldReturnFalse()
        {
            //Arrange
            var user = new User { Username = "lpinedo", Password = "password" };
            var username = "lpinedo";
            var password = "asdasd";
            //Act
            userRepository.AddUser(user);
            var result = authenticationService.Authenticate(username, password);
            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void Register_ShouldSucceed_WhenUserDoesNotExist()
        {
            //Arrange
            var username = "lpinedo";
            var password = "password";
            //Act
            authenticationService.Register(username, password);
            var result = userRepository.GetUserByUsername(username);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Username);
        }
        [TestMethod]
        public void Register_ShouldFail_WhenUserAlreadyExists()
        {
            //Arrange
            var user = new User { Username = "lpinedo", Password = "password" };
            var username = "lpinedo";
            var password = "password";
            //Act
            userRepository.AddUser(user);
            //Assert
            Assert.ThrowsException<ArgumentException>(() => authenticationService.Register(username, password));// Assert.ThrowsException<ArgumentException>(   {Codigo que deberia lanzar una exception}  );
        }
        [TestMethod]
        public void AssignRoleToUser_ShouldSucceed()
        {
            //Arrange
            var user = new User { Username = "lpinedo", Password = "password" };
            var username = "lpinedo";
            var roleName = "Admin";
            //Act
            userRepository.AddUser(user);
            authenticationService.AssignRoleToUser(username, roleName);
            var result = userRepository.GetUserByUsername(username);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Username);
            Assert.AreEqual(1, user.Roles.Count);
        }
        [TestMethod]
        public void AssignRoleToUser_ShouldExceptionForUser()
        {
            //Arrange
            var username = "lpinedo";
            var roleName = "Admin";
            //Act
            //Assert
            Assert.ThrowsException<ArgumentException>(() => authenticationService.AssignRoleToUser(username, roleName));
        }
        [TestMethod]
        public void AssignRoleToUser_ShouldExceptionForRole()
        {
            //Arrange
            var user = new User { Username = "lpinedo", Password = "password" };
            var username = "lpinedo";
            var roleName = "INEXISTENTE";
            //Act
            userRepository.AddUser(user);
            //Assert
            Assert.ThrowsException<ArgumentException>(() => authenticationService.AssignRoleToUser(username, roleName));
        }
    }
}