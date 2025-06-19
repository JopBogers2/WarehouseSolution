using Moq;
using Warehouse.App.MVVM.Models;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.ViewModels;
using Warehouse.Shared.Models;

namespace Warehouse.Tests
{
    public class DashboardViewModelTest
    {
        [Theory]
        [InlineData("magento", "lampenlicht.nl", "500013", "100003", "HAP", "EUR")]
        [InlineData("allegro", "lampyiswiatlo.pl", "600013", "200003", "WRO", "PLN")]
        [InlineData("bol", "lampenlicht.nl", "700013", "300003", "HAP", "EUR")]
        public void LoadsRmasFromApiService_MapsPropertiesCorrectly(string platform, string channel, string orderId, string returnRequestId, string distributionCenter, string currency)
        {
            // Arrange
            var mockkApiService = new Mock<IApiService>();
            var testRmas = new List<ReturnMerchandiseAuthorizationDto>
                {
                    new ReturnMerchandiseAuthorizationDto
                    {
                        Platform = platform,
                        Channel = channel,
                        OrderId = orderId,
                        ReturnRequestId = returnRequestId,
                        DistributionCenter = distributionCenter,
                        Currency = currency,
                        TrackAndTrace = new List<string> {},
                        Lines = new List<ReturnMerchandiseAuthorizationLineDto>{}
                    }
                };
            mockkApiService.Setup(x => x.SearchRmasAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(new ApiResult<PagedResult<ReturnMerchandiseAuthorizationDto>>
                {
                    Data = new PagedResult<ReturnMerchandiseAuthorizationDto>
                    {
                        Items = testRmas,
                        PageNumber = 1,
                        PageSize = 1,
                        TotalCount = 1
                    }
                });

            // Act
            var viewModel = new DashboardViewModel(mockkApiService.Object);

            // Assert
            Assert.NotNull(viewModel.Rmas);
            Assert.Single(viewModel.Rmas);
            Assert.Equal(platform, viewModel.Rmas[0].Platform);
            Assert.Equal(channel, viewModel.Rmas[0].Channel);
            Assert.Equal(orderId, viewModel.Rmas[0].OrderId);
            Assert.Equal(returnRequestId, viewModel.Rmas[0].ReturnRequestId);
            Assert.Equal(distributionCenter, viewModel.Rmas[0].DistributionCenter);
            Assert.Equal(currency, viewModel.Rmas[0].Currency);
            Assert.Empty(viewModel.Rmas[0].TrackAndTrace);
            Assert.Empty(viewModel.Rmas[0].Lines);
            Assert.Equal(1, viewModel.RmaCount);
            Assert.Equal(1, viewModel.TotalCount);
            Assert.Equal(1, viewModel.TotalPages);
            Assert.Equal(1, viewModel.PageNumber);
            Assert.Equal(20, viewModel.PageSize);
            Assert.Null(viewModel.ErrorMessage);
            Assert.False(viewModel.IsLoading);
        }

        [Fact]
        public void SetsErrorMessage_WhenApiServiceFails()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var errorMsg = "API failure";
            mockApiService.Setup(x => x.SearchRmasAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
                .ReturnsAsync(new ApiResult<PagedResult<ReturnMerchandiseAuthorizationDto>>
                {
                    ErrorMessage = errorMsg,
                    Data = null
                });

            // Act
            var viewModel = new DashboardViewModel(mockApiService.Object);

            // Assert
            Assert.Equal(errorMsg, viewModel.ErrorMessage);
            Assert.Empty(viewModel.Rmas);
            Assert.Equal(0, viewModel.RmaCount);
            Assert.Equal(0, viewModel.TotalCount);
            Assert.Equal(1, viewModel.TotalPages);
            Assert.False(viewModel.IsLoading);
        }
    }
}