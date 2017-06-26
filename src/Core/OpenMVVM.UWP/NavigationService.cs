namespace OpenMVVM.UWP
{
    using System.Linq;
    using System.Threading.Tasks;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;

    using global::Windows.UI.Core;
    using global::Windows.UI.Xaml;
    using global::Windows.UI.Xaml.Controls;

    public class NavigationService : INavigationService
    {   
        private readonly IInstanceFactory instanceFactory;

        private readonly ILifecycleService lifecycleService;

        private string currentViewName;

        public NavigationService(IInstanceFactory instanceFactory, ILifecycleService lifecycleService)
        {
            this.instanceFactory = instanceFactory;
            this.lifecycleService = lifecycleService;
        }

        public async Task<bool> NavigateTo(string pageName, object parameter = null, bool removeBackEntry = false)
        {
            if (!string.IsNullOrEmpty(this.currentViewName))
            {
                var navigationEventHandlerArgs = new NavigationEventHandlerArgs(
                    this.currentViewName,
                    NavigationType.Forward,
                    parameter);

                this.OnNavigatingFrom(navigationEventHandlerArgs);
                Task<NavigationResult>[] tasks = navigationEventHandlerArgs.Tasks.ToArray();

                NavigationResult[] navigationResults = await Task.WhenAll(tasks);

                if (navigationResults.Any(res => res == NavigationResult.Cancel))
                {
                    return false;
                }
            }

            var page = this.instanceFactory.GetInstanceByKey<Page>(pageName);

            this.Frame.Navigate(page.GetType(), parameter);

            if (removeBackEntry)
            {
                this.Frame.BackStack.RemoveAt(this.Frame.BackStackDepth - 1);
            }

            string desiredViewName = this.Frame.Content?.GetType().Name;

            if (!string.IsNullOrEmpty(desiredViewName))
            {
                this.currentViewName = desiredViewName;
                this.OnNavigatingTo(new NavigationEventHandlerArgs(desiredViewName, NavigationType.Forward, parameter));
            }

            return true;
        }

        public async Task<bool> GoBack()
        {
            if (this.CanGoBack)
            {
                var navigationEventHandlerArgs = new NavigationEventHandlerArgs(this.currentViewName, NavigationType.Backward, null);

                this.OnNavigatingFrom(navigationEventHandlerArgs);
                Task<NavigationResult>[] tasks = navigationEventHandlerArgs.Tasks.ToArray();

                NavigationResult[] navigationResults = await Task.WhenAll(tasks);

                if (navigationResults.Any(res => res == NavigationResult.Cancel))
                {
                    return false;
                }

                this.Frame.GoBack();

                this.currentViewName = this.Frame.Content?.GetType().Name;

                if (!string.IsNullOrEmpty(this.currentViewName))
                {
                    this.OnNavigatingTo(new NavigationEventHandlerArgs(this.currentViewName, NavigationType.Backward, null));
                }
                else
                {
                    if (!this.lifecycleService.TryCloseApplication())
                    {

                    }
                }

                return true;
            }
            else
            {
                if (!this.lifecycleService.TryCloseApplication())
                {

                }
            }

            return false;
        }

        public bool CanGoBack
        {
            get
            {
                return this.Frame != null && this.Frame.CanGoBack;
            }
        }

        public event NavigationEventHandler NavigatingTo;

        public event NavigationEventHandler NavigatingFrom;

        protected virtual void OnNavigatingFrom(NavigationEventHandlerArgs e)
        {
            NavigationEventHandler handler = this.NavigatingFrom;
            handler?.Invoke(this, e);
        }

        protected virtual void OnNavigatingTo(NavigationEventHandlerArgs e)
        {
            NavigationEventHandler handler = this.NavigatingTo;
            handler?.Invoke(this, e);
        }

        private Frame Frame => (Frame)Window.Current.Content;
    }
}
