namespace Warehouse.App.MVVM.Services
{
    public interface INavigationService
    {
        Task GoToAsync(string route);
        Task PushAsync(Page page);
        Task PopAsync();
        void RemovePage(Page page);
    }

    public class NavigationService : INavigationService
    {
        public Task GoToAsync(string route)
        {
            return Shell.Current.GoToAsync(route);
        }

        public Task PushAsync(Page page)
        {
            return Shell.Current.Navigation.PushAsync(page);
        }

        public Task PopAsync()
        {
            return Shell.Current.Navigation.PopAsync();
        }

        public void RemovePage(Page page)
        {
            Shell.Current.Navigation.RemovePage(page);
        }
    }
}
