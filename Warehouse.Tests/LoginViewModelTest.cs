using Moq;
using Warehouse.App.MVVM.Models;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.ViewModels;

namespace Warehouse.Tests
{
    public class LoginViewModelTest
    {
        [Fact]
        public async Task SetsErrorMessage_WhenLoginFails()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var mockNavigationService = new Mock<INavigationService>();
            var errorMsg = "Invalid credentials";
            mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(errorMsg);

            var viewModel = new LoginViewModel(mockAuthService.Object, mockNavigationService.Object)
            {
                Username = "user",
                Password = "wrongpass"
            };

            // Act
            await viewModel.LoginCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal(errorMsg, viewModel.ErrorMessage);
            Assert.False(viewModel.Loading);
        }

        [Fact]
        public async Task NavigatesToDashboard_WhenLoginSucceeds()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var mockNavigationService = new Mock<INavigationService>();
            mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync((string?)null);

            var viewModel = new LoginViewModel(mockAuthService.Object, mockNavigationService.Object)
            {
                Username = "user",
                Password = "correctpass"
            };

            // Act
            await viewModel.LoginCommand.ExecuteAsync(null);

            // Assert
            Assert.True(string.IsNullOrEmpty(viewModel.ErrorMessage));
            Assert.False(viewModel.Loading);
            mockNavigationService.Verify(x => x.GoToAsync("//DashboardPage", null), Times.Once);
        }
    }
}
