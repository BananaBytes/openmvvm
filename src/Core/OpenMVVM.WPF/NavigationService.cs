namespace OpenMVVM.WPF
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;

    public class NavigationService : INavigationService
    {
        private readonly IInstanceFactory instanceFactory;

        private readonly ILifecycleService lifecycleService;

        private readonly Frame frame;

        private string currentView;

        public NavigationService(IInstanceFactory instanceFactory, ILifecycleService lifecycleService)
        {
            this.lifecycleService = lifecycleService;
            this.instanceFactory = instanceFactory;

            this.frame = (Frame)Application.Current.MainWindow.FindName("Frame");
        }

        public event NavigationEventHandler NavigatingTo;

        public event NavigationEventHandler NavigatingFrom;

        public bool CanGoBack => true;

        public async Task<bool> NavigateTo(string pageName, object parameter = null, bool removeBackEntry = false)
        {
            if (this.currentView != null)
            {
                var navigationEventHandlerArgs = new NavigationEventHandlerArgs(
                    this.currentView,
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

            page.Title = pageName;
            this.frame.Navigate(page, parameter);

            if (removeBackEntry)
            {
                this.frame.Navigated += this.RemoveBackEntry;
            }

            this.currentView = this.frame.Content?.GetType().Name;
            this.OnNavigatingTo(new NavigationEventHandlerArgs(this.currentView, NavigationType.Forward, parameter));

            return true;
        }

        public async Task<bool> GoBack()
        {
            if (this.CanGoBack)
            {
                var navigationEventHandlerArgs = new NavigationEventHandlerArgs(this.currentView, NavigationType.Backward, null);
                this.OnNavigatingFrom(navigationEventHandlerArgs);
                Task<NavigationResult>[] tasks = navigationEventHandlerArgs.Tasks.ToArray();

                NavigationResult[] navigationResults = await Task.WhenAll(tasks);

                if (navigationResults.Any(res => res == NavigationResult.Cancel))
                {
                    return false;
                }

                this.frame.Navigated += this.FrameNavigatedBack;

                this.frame.GoBack();

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

        private void FrameNavigatedBack(object sender, NavigationEventArgs e)
        {
            this.frame.Navigated -= this.FrameNavigatedBack;

            this.currentView = this.frame.Content?.GetType().Name;

            if (this.currentView != null)
            {
                this.OnNavigatingTo(new NavigationEventHandlerArgs(this.currentView, NavigationType.Backward, null));
            }
            else
            {
                if (!this.lifecycleService.TryCloseApplication())
                {

                }
            }
        }

        private void RemoveBackEntry(object sender, NavigationEventArgs e)
        {
            this.frame.RemoveBackEntry();
            this.frame.Navigated -= this.RemoveBackEntry;
        }
    }
}
