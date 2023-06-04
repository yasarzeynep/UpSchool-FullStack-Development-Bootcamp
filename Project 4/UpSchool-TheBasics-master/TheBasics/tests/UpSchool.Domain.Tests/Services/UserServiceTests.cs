using FakeItEasy;
using System.Collections.Generic;
using System.Linq.Expressions;
using UpSchool.Domain.Data;
using UpSchool.Domain.Entities;
using UpSchool.Domain.Services;

namespace UpSchool.Domain.Tests.Services
{
    public class UserServiceTests
    {
        [Fact] // 0

        public async Task GetUser_ShouldGetUserWithCorrectId()
        {
            // Arrange 
            var userRepositoryMock = A.Fake<IUserRepository>();

            Guid userId = new Guid("8f319b0a-2428-4e9f-b7c6-ecf78acf00f9");

            var cancellationSource = new CancellationTokenSource();

            //Act 
            var expectedUser = new User()
            {
                Id = userId
            };

            A.CallTo(() => userRepositoryMock.GetByIdAsync(userId, cancellationSource.Token))
                .Returns(Task.FromResult(expectedUser));

            IUserService userService = new UserManager(userRepositoryMock);

            var user = await userService.GetByIdAsync(userId, cancellationSource.Token);

            //Assert
            Assert.Equal(expectedUser, user);
        }

        [Fact] // 1

        public async Task AddAsync_ShouldThrowException_WhenEmailIsEmptyOrNull()
        {   //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();
            var cancellationSource = new CancellationTokenSource();
            //Act
            IUserService userService = new UserManager(userRepositoryMock);
            //Assert 
            //xUnit.net Assert.Throws method used.
            await Assert.ThrowsAsync<ArgumentException>(() => userService.AddAsync("Ali", "Bak", 7, null, cancellationSource.Token)); //null
            await Assert.ThrowsAsync<ArgumentException>(() => userService.AddAsync("İpek", "Tut", 8, String.Empty, cancellationSource.Token));//empty
        }
        [Fact] // 2

        public async Task AddAsync_ShouldReturn_CorrectUserId()
        {   //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();
            Guid userId = new Guid("8f319b0a-2428-4e9f-b7c6-ecf78acf00f9");
            var cancellationSource = new CancellationTokenSource();
            //Act
            var expectedUser = new User()
            {
                Id = userId
            };

            A.CallTo(() => userRepositoryMock.GetByIdAsync(userId, cancellationSource.Token))
                .Returns(Task.FromResult(expectedUser));
            IUserService userService = new UserManager(userRepositoryMock);
            var userGuid = await userService.AddAsync("Zeynep", "Yasar", 28, "zynpyasar@hotmail.com", cancellationSource.Token);
            //Assert 
            //xUnit.net IsType<T>,NotEqual and NotNull methods used.
            Assert.IsType<Guid>(userGuid);
            Assert.NotEqual(Guid.Empty, userGuid);
            Assert.NotNull(userGuid);
        }
        [Fact] // 3

        public async Task DeleteAsync_ShouldReturnTrue_WhenUserExists()
        {   //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();
            //Act
            Guid id = Guid.NewGuid();
            A.CallTo(() => userRepositoryMock.DeleteAsync(A<Expression<Func<User, bool>>>.Ignored, cancellationSource.Token))
                .Returns(Task.FromResult(1));

            IUserService userService = new UserManager(userRepositoryMock);
            var acces = await userService.DeleteAsync(id, cancellationSource.Token);
            //Assert
            //xUnit.net True method used.
            Assert.True(acces);

        }
        [Fact] // 4

        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesntExists()
        {   //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();
            //Act
            Guid id = Guid.NewGuid();

            IUserService userService = new UserManager(userRepositoryMock);
            
            //Assert
            //xUnit.net Assert.Throws method used.
            await Assert.ThrowsAsync<ArgumentException>(() => userService.DeleteAsync(Guid.Empty, cancellationSource.Token));

        }
        [Fact] // 5

        public async Task UpdateAsync_ShouldThrowException_WhenUserIdIsEmpty()
        {   //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();
            //Act
            var user = new User()
            {
                Id = Guid.Empty
            };

            IUserService userService = new UserManager(userRepositoryMock);

            //Assert
            //xUnit.net Assert.Throws method used.
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(user, cancellationSource.Token));

        }
        [Fact] // 6

        public async Task UpdateAsync_ShouldThrowException_WhenUserEmailEmptyOrNull()
        {   //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();
            //Act
            var userNull = new User() // for null email
            {
                Id = Guid.NewGuid()
            };
            var userEmpty = new User() // for empty email
            {
                Email = String.Empty 

            };
            IUserService userService = new UserManager(userRepositoryMock);
            //Assert
            //xUnit.net Assert.Throws method used.
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(userNull,cancellationSource.Token)); //null
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(userEmpty,cancellationSource.Token));//empty

        }
        [Fact] // 7

        public async Task GetAllAsync_ShouldReturn_UserListWithAtLeastTwoRecords()
        {   //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();
            //Act
            List<User> userList = new List<User>()
            {
                new User(){ Id = Guid.NewGuid() },
               // new User(){ Id = Guid.NewGuid(),FirstName = "Zeynep", LastName = "Yasar", Age = 28, Email = "zynpsema@gmail.com" },

            };

            IUserService userService = new UserManager(userRepositoryMock);
            //Assert
            //xUnit.net alternative: Assert.True(x > y) used.
            var UserList = await userService.GetAllAsync (cancellationSource.Token);
            Assert.True(UserList.Count >= 2);
          

        }
    }
}