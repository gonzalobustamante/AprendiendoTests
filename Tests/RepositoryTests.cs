using Core;

namespace Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private IRoleRepository rolRepository;

        [TestInitialize]
        public void Setup()
        {
            rolRepository = new RoleRepository();
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
    }
}