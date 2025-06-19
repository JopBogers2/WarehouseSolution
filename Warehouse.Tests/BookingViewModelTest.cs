using Moq;
using Warehouse.App.MVVM.Models;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.ViewModels;
using Warehouse.Shared.Models;

namespace Warehouse.Tests
{
    public class BookingViewModelTest
    {
        [Fact]
        public async Task SetsErrorMessage_WhenGetRmaByTrackAndTraceFails()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockPageService = new Mock<IPageService>();
            var mockNavigationService = new Mock<INavigationService>();
            var errorMsg = "RMA not found";
            mockApiService.Setup(x => x.GetRmaByTrackAndTraceAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApiResult<ReturnMerchandiseAuthorizationDto>
                {
                    ErrorMessage = errorMsg,
                    Data = null
                });

            var viewModel = new BookingViewModel(mockApiService.Object, mockServiceProvider.Object, mockPageService.Object, mockNavigationService.Object)
            {
                TrackAndTrace = "SOME_CODE"
            };

            // Act
            await viewModel.SearchRmaCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal(errorMsg, viewModel.ErrorMessage);
            Assert.False(viewModel.IsLoading);
        }

        [Fact]
        public async Task NavigatesToRmaDetailPage_WhenGetRmaByTrackAndTraceSucceeds()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var MockPageService = new Mock<IPageService>();
            var mockNavigationService = new Mock<INavigationService>();
            var rmaDto = new ReturnMerchandiseAuthorizationDto
            {
                Platform = "platform",
                Channel = "channel",
                OrderId = "orderId",
                ReturnRequestId = "returnRequestId",
                DistributionCenter = "dc",
                Currency = "EUR",
                TrackAndTrace = new List<string>(),
                Lines = new List<ReturnMerchandiseAuthorizationLineDto>()
            };

            mockApiService.Setup(x => x.GetRmaByTrackAndTraceAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApiResult<ReturnMerchandiseAuthorizationDto>
                {
                    Data = rmaDto
                });

            MockPageService.Setup(x => x.CreateRmaDetailPage(It.IsAny<RmaDetailViewModel>()))
                .Returns(null as Page);

            var viewModel = new BookingViewModel(mockApiService.Object, mockServiceProvider.Object, MockPageService.Object, mockNavigationService.Object)
            {
                TrackAndTrace = "SOME_CODE"
            };

            // Act
            await viewModel.SearchRmaCommand.ExecuteAsync(null);

            // Assert
            Assert.Null(viewModel.ErrorMessage);
            Assert.False(viewModel.IsLoading);
        }
    }
}
