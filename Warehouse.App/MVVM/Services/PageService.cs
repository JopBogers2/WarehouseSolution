using Warehouse.App.MVVM.ViewModels;
using Warehouse.App.MVVM.Views;

namespace Warehouse.App.MVVM.Services
{
    public interface IPageService
    {
        public Page CreateRmaDetailPage(RmaDetailViewModel viewModel);
    }
    public class PageService : IPageService
    {
        public Page CreateRmaDetailPage(RmaDetailViewModel viewModel) =>
            new RmaDetailPage(viewModel);
    }
}
