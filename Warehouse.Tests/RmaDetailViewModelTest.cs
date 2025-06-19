using Moq;
using Warehouse.App.MVVM.Models;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.ViewModels;
using Warehouse.Shared.Models;

namespace Warehouse.Tests
{
    public class RmaDetailViewModelTest
    {
        private static ReturnMerchandiseAuthorizationDto CreateValidRmaDto()
        {
            return new ReturnMerchandiseAuthorizationDto
            {
                Platform = "platform",
                Channel = "channel",
                OrderId = "orderId",
                ReturnRequestId = "returnRequestId",
                DistributionCenter = "dc",
                Currency = "EUR",
                TrackAndTrace = new List<string>(),
                Lines = new List<ReturnMerchandiseAuthorizationLineDto>
                {
                    new ReturnMerchandiseAuthorizationLineDto
                    {
                        LineId = 1,
                        ArticleCode = "A1",
                        Quantity = 1,
                        Reason = "reason",
                        Resolution = "resolution",
                    }
                }
            };
        }

        [Fact]
        public void DeleteLine_RemovesLineFromCollection()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var mockNavigationService = new Mock<INavigationService>();
            var rmaDto = CreateValidRmaDto();
            var viewModel = new RmaDetailViewModel(mockApiService.Object, mockNavigationService.Object, rmaDto);

            var lineToDelete = viewModel.Lines.First();

            // Act
            viewModel.DeleteLineCommand.Execute(lineToDelete);

            // Assert
            Assert.Empty(viewModel.Lines);
        }

        [Fact]
        public async Task BookReturn_SetsErrorMessage_WhenNoLines()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var mockNavigationService = new Mock<INavigationService>();
            var rmaDto = CreateValidRmaDto();
            rmaDto.Lines.Clear();
            var viewModel = new RmaDetailViewModel(mockApiService.Object, mockNavigationService.Object, rmaDto);

            // Act
            await viewModel.BookReturnCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal("At least one line is required to book an RMA.", viewModel.ErrorMessage);
        }

        [Fact]
        public async Task BookReturn_SetsErrorMessage_WhenLineQuantityIsZero()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var mockNavigationService = new Mock<INavigationService>();
            var rmaDto = CreateValidRmaDto();
            rmaDto.Lines[0].Quantity = 0;
            var viewModel = new RmaDetailViewModel(mockApiService.Object, mockNavigationService.Object, rmaDto);

            // Act
            await viewModel.BookReturnCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal("All lines must have a quantity greater than zero.", viewModel.ErrorMessage);
        }

        [Fact]
        public async Task BookReturn_SetsErrorMessage_WhenLineConditionOrResolutionMissing()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var mockNavigationService = new Mock<INavigationService>();
            var rmaDto = CreateValidRmaDto();

            var viewModel = new RmaDetailViewModel(mockApiService.Object, mockNavigationService.Object, rmaDto);

            // Act
            await viewModel.BookReturnCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal("All lines must have a Condition and Resolution specified.", viewModel.ErrorMessage);
        }

        [Fact]
        public async Task BookReturn_PopsPage_WhenApiCallSucceeds()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var mockNavigationService = new Mock<INavigationService>();

            var rmaDto = CreateValidRmaDto();

            mockApiService.Setup(x => x.PostReturn(It.IsAny<ReturnDto>()))
                .ReturnsAsync(new ApiResult<ReturnDto> { Data = new ReturnDto { Platform = "platform", Channel = "channel", OrderId = "orderId", ReturnRequestId = "returnRequestId", Lines = new List<ReturnLineDto>() } });

            var viewModel = new RmaDetailViewModel(mockApiService.Object, mockNavigationService.Object, rmaDto);

            viewModel.Lines.All(line =>
            {
                line.Condition = "Good";
                line.Resolution = "Refund";
                return true;
            });

            // Act
            await viewModel.BookReturnCommand.ExecuteAsync(null);

            // Assert
            Assert.Null(viewModel.ErrorMessage);
            mockNavigationService.Verify(n => n.PopAsync(), Times.Once);
        }

        [Fact]
        public async Task BookReturn_SetsErrorMessage_WhenApiCallFails()
        {
            // Arrange
            var mockApiService = new Mock<IApiService>();
            var mockNavigationService = new Mock<INavigationService>();
            var rmaDto = CreateValidRmaDto();

            mockApiService.Setup(x => x.PostReturn(It.IsAny<ReturnDto>()))
                .ReturnsAsync(new ApiResult<ReturnDto> { ErrorMessage = "API error" });

            var viewModel = new RmaDetailViewModel(mockApiService.Object, mockNavigationService.Object, rmaDto);

            viewModel.Lines.All(line =>
            {
                line.Condition = "Good";
                line.Resolution = "Refund";
                return true;
            });

            // Act
            await viewModel.BookReturnCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal("API error", viewModel.ErrorMessage);
        }
    }
}
