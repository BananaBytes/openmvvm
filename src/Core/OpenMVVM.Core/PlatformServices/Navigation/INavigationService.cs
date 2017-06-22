namespace OpenMVVM.Core.PlatformServices.Navigation
{
    using System.Threading.Tasks;

    public interface INavigationService
    {
        event NavigationEventHandler NavigatingTo;

        event NavigationEventHandler NavigatingFrom;

        bool CanGoBack { get; }

        Task<bool> NavigateTo(string pageName, object parameter = null, bool removeBackEntry = false);

        Task<bool> GoBack();
    }
}
