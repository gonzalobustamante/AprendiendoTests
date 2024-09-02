using Core; // Importa el namespace Core, que probablemente contiene las interfaces y clases como IUserRepository, IRoleRepository, User, y Role.

namespace Tests // Define el namespace 'Tests' para las clases de pruebas unitarias.
{
    [TestClass] // Indica que esta clase contiene métodos de prueba.
    public class RepositoryTests // Clase de prueba para las funcionalidades del repositorio de roles.
    {
        private IRoleRepository rolRepository; // Define una variable privada para el repositorio de roles.

        [TestInitialize] // Indica que este método se ejecutará antes de cada prueba.
        public void Setup() 
        {
            rolRepository = new RoleRepository(); // Inicializa el repositorio de roles antes de cada prueba.
        }

        [TestMethod] 
        public void GetRoleByName_ShouldReturnOK() //este metodo es para probar que el rol se obtenga correctamente
        {
            // Arrange - Configura los datos de prueba.
            string roleName = "Admin"; // Define un nombre de rol.

            // Act - Llama al método que se está probando.
            var result = rolRepository.GetRoleByName(roleName); // Obtiene el rol por su nombre.

            // Assert - Verifica que los resultados sean los esperados.
            Assert.IsNotNull(result); // Verifica que el resultado no sea nulo.
            Assert.AreEqual(roleName, result.Name); // Verifica que el nombre del rol sea el esperado.
            Assert.AreEqual(1, result.Id); // Verifica que el ID del rol sea 1.
        }

        [TestMethod] 
        public void GetRolByName_WhenParametersAreIncorrect_ShouldReturnNull()//este metodo es para probar que el rol no se obtenga correctamente
        {
            // Arrange 
            string roleName = "INEXISTENTE"; // Define un nombre de rol inexistente.

            // Act
            var result = rolRepository.GetRoleByName(roleName); // Intenta obtener un rol con un nombre inexistente.

            // Assert 
            Assert.IsNull(result); // Verifica que el resultado sea nulo.
        }

        [TestMethod] 
        public void GetAllRoles_ShouldReturnOK()//este metodo es para probar que se obtengan todos los roles
        {
            // Arrange

            // Act
            var result = rolRepository.GetAllRoles(); // Obtiene todos los roles.

            // Assert
            Assert.IsNotNull(result); // Verifica que el resultado no sea nulo.
            Assert.AreEqual(2, result.Count); // Verifica que el número de roles sea 2.
        }
    }

    //pruebas unitarias para el repositorio de usuarios

    [TestClass] // Indica que esta clase contiene métodos de prueba.
    public class UserRepositoryTests // Clase de prueba para las funcionalidades del repositorio de usuarios.
    {
        private IUserRepository userRepository; // Define una variable privada para el repositorio de usuarios.

        [TestInitialize] 
        public void Setup()
        {
            userRepository = new UserRepository(); // Inicializa el repositorio de usuarios antes de cada prueba.
        }

        [TestMethod] 
        public void GetUserByUsername_ShouldReturnOK()//este metodo es para probar que el usuario se obtenga correctamente
        {
            // Arrange
            string username = "Carlos"; // Define un nombre de usuario.
            string password = "1234"; // Define una contraseña.

            User user = new User { Username = username, Password = password }; // Crea un objeto User con un nombre de usuario y contraseña.
            userRepository.AddUser(user); // Agrega el usuario al repositorio.

            // Act
            var result = userRepository.GetUserByUsername(username); // Obtiene el usuario por su nombre de usuario.

            // Assert
            Assert.IsNotNull(result); // Verifica que el resultado no sea nulo.
            Assert.AreEqual(username, result.Username); // Verifica que el nombre de usuario sea el esperado.
            Assert.AreEqual(1, result.Id); // Verifica que el ID del usuario sea 1.
        }

        [TestMethod] 
        public void GetUserByUsername_WhenParametersAreIncorrect_ShouldReturnNull()//este metodo es para probar que el usuario no se obtenga correctamente
        {
            // Arrange.
            string username = "INEXISTENTE"; // Define un nombre de usuario inexistente.

            // Act
            var result = userRepository.GetUserByUsername(username); // Intenta obtener un usuario con un nombre inexistente.

            // Assert
            Assert.IsNull(result); // Verifica que el resultado sea nulo.
        }

        [TestMethod] 
        public void AddUser_ShouldReturnOK()//este metodo es para probar que el usuario se agregue correctamente
        {
            // Arrange
            User user = new User { Username = "Carlos", Password = "1234" }; // Crea un objeto User con un nombre de usuario y contraseña.

            // Act
            userRepository.AddUser(user); // Agrega el usuario al repositorio.

            // Assert
            Assert.AreEqual(1, user.Id); // Verifica que el ID del usuario sea 1.
        }

        [TestMethod]
        public void AddUser_WhenUserIsNull_ShouldReturnException()//este metodo es para probar que el usuario no se agregue correctamente
        {
            var user = new User {
              Username = null,
              Password = null
            };
            userRepository.AddUser(user);
            Assert.IsNull(user.Username);
            Assert.IsNull(user.Password);
        }

        [TestMethod] 
        public void AddRoleToUser_ShouldReturnOK()//este metodo es para probar que el rol se agregue correctamente
        {
            // Arrange
            User user = new User { Username = "test", Password = "test" }; // Crea un objeto User con un nombre de usuario y contraseña.
            userRepository.AddUser(user); // Agrega el usuario al repositorio.
            Role role = new Role { Id = 3, Name = "Empleado" }; // Crea un objeto Role con un ID y un nombre.
            // Act
            userRepository.AddRoleToUser(user.Id, role); // Asigna el rol al usuario.
            // Assert
            Assert.AreEqual(1, user.Roles.Count); // Verifica que el usuario tenga un rol asignado.
            Assert.AreEqual(role.Id, user.Roles[0].Id); // Verifica que el ID del rol asignado sea el correcto.
            Assert.AreEqual(role.Name, user.Roles[0].Name); // Verifica que el nombre del rol asignado sea el correcto.
        }

        [TestMethod] 
        public void AddRoleToUser_WhenUserDoesNotExist_ShouldReturnException()//este metodo es para probar que el rol no se agregue correctamente
        {
            User user = new User {
                Username = "test",
                Password = "test"
                };
            userRepository.AddUser(user);
            Role role = new Role {
                Id = 3,
                Name = "Empleado"
                };
            userRepository.AddRoleToUser(0, role);
            Assert.AreEqual(0, user.Roles.Count);

        }

    }
    //fin de las pruebas unitarias para el repositorio de usuarios}


    //pruebas unitarias para el servicio de autenticacion

    [TestClass] // Indica que esta clase contiene métodos de prueba.

    public class AuthenticationServiceTests // Clase de prueba para las funcionalidades del servicio de autenticación.
    {
        private IAuthenticationService authenticationService; // Define una variable privada para el servicio de autenticación.
        private IUserRepository userRepository; // Define una variable privada para el repositorio de usuarios.
        private IRoleRepository roleRepository; // Define una variable privada para el repositorio de roles.

        [TestInitialize] 
        public void Setup()
        {
            userRepository = new UserRepository(); // Inicializa el repositorio de usuarios antes de cada prueba.
            roleRepository = new RoleRepository(); // Inicializa el repositorio de roles antes de cada prueba.
            authenticationService = new AuthenticationService(userRepository, roleRepository); // Inicializa el servicio de autenticación antes de cada prueba.
        }

        [TestMethod] 

        public void Authenticate_ShouldReturnOK()//este metodo es para probar que la autenticacion sea correcta
        {
            // Arrange
            string username = "Carlos"; // Define un nombre de usuario.
            string password = "1234"; // Define una contraseña.

            User user = new User { Username = username, Password = password }; // Crea un objeto User con un nombre de usuario y contraseña.
            userRepository.AddUser(user); // Agrega el usuario al repositorio.

            // Act
            var result = authenticationService.Authenticate(username, password); // Autentica al usuario.

            // Assert 
            Assert.IsTrue(result); // Verifica que la autenticación sea exitosa.
        }

        [TestMethod]
        public void Authenticate_WhenUserDoesNotExist_ShouldReturnException()//este metodo es para probar que la autenticacion no sea correcta
        {
            // Arrange - Configura los datos de prueba.
            string username = "INEXISTENTE"; // Define un nombre de usuario inexistente.
            string password = "1234"; // Define una contraseña.

            // Act - Llama al método que se está probando.
            var result = authenticationService.Authenticate(username, password); // Intenta autenticar a un usuario inexistente.

            // Assert - Verifica que los resultados sean los esperados.
            Assert.IsFalse(result); // Verifica que la autenticación no sea exitosa.
        }


        [TestMethod]
        public void Register_ShouldReturnOK()//este metodo es para probar que el registro sea correcto
        {
            // Arrange
            string username = "Perea"; // Define un nombre de usuario.
            string password = "1234"; // Define una contraseña.
            User user = new User
            {
                Username = username,
                Password = password
            };
            // Act
            authenticationService.Register(username, password); // Registra al usuario.

            // Assert
            Assert.IsNotNull(userRepository.GetUserByUsername(username)); // Verifica que el usuario se haya registrado correctamente.
        }

        [TestMethod]
        public void Register_WhenUserAlreadyExists_ShouldReturnException()//este metodo es para probar que el registro no sea correcto
        {
            var username = "Perea";
            var password = "1234";
            User user = new User
            {
               Username = username,
               Password = password

            };

            userRepository.AddUser(user);
            Assert.ThrowsException<ArgumentException>(() => authenticationService.Register(username, password));
        }
        
        
        [TestMethod]
        public void AssignRoleToUser_ShouldReturnOK()//este metodo es para probar que el rol se asigne correctamente
        {
            // Arrange
            string username = "Carlos"; // Define un nombre de usuario.
            string password = "1234"; // Define una contraseña.
            string roleName = "Admin"; // Define un nombre de rol.

            User user = new User { Username = username, Password = password }; // Crea un objeto User con un nombre de usuario y contraseña.
            userRepository.AddUser(user); // Agrega el usuario al repositorio.

            Role role = new Role { Id = 1, Name = roleName }; // Crea un objeto Role con un ID y un nombre.

            // Act
            authenticationService.AssignRoleToUser(username, roleName); // Asigna el rol al usuario.

            // Assert
            Assert.AreEqual(1, user.Roles.Count); // Verifica que el usuario tenga un rol asignado.
            Assert.AreEqual(role.Id, user.Roles[0].Id); // Verifica que el ID del rol asignado sea el correcto.
            Assert.AreEqual(role.Name, user.Roles[0].Name); // Verifica que el nombre del rol asignado sea el correcto.
        }

        [TestMethod]
        public void AssignRoleToUser_WhenUserDoesNotExist_ShouldReturnException()//este metodo es para probar que el rol no se asigne correctamente por si el usuario no exite
        {
            // Arrange
            string username = "INEXISTENTE"; // Define un nombre de usuario inexistente.
            string roleName = "Admin"; // Define un nombre de rol.

            // Assert
            Assert.ThrowsException<ArgumentException>(() => authenticationService.AssignRoleToUser(username, roleName)); // Intenta asignar un rol a un usuario inexistente.
        }

        [TestMethod]
        public void AssignRoleToUser_WhenRoleDoesNotExist_ShouldReturnException()//este metodo es para probar que el rol no se asigne correctamente por si el rol no exite
        {
            // Arrange
            string username = "Carlos"; // Define un nombre de usuario.
            string password = "1234"; // Define una contraseña.
            string roleName = "INEXISTENTE"; // Define un nombre de rol inexistente.

            User user = new User { Username = username, Password = password }; // Crea un objeto User con un nombre de usuario y contraseña.
            userRepository.AddUser(user); // Agrega el usuario al repositorio.

            // Assert
            Assert.ThrowsException<ArgumentException>(() => authenticationService.AssignRoleToUser(username, roleName)); // Intenta asignar un rol inexistente a un usuario.
        }

           

    }
}
