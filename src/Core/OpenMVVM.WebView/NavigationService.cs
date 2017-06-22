namespace OpenMVVM.WebView
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;

    public class NavigationService : INavigationService
    {
        private readonly ILifecycleService lifecycleService;

        public IBridge Bridge { get; set; }

        private string currentView;

        //private MainPage<TLocator> mainPage = (MainPage<TLocator>)Window.Current.Content;

        private List<string> backStack = new List<string>() { };

        public NavigationService(ILifecycleService lifecycleService)
        {
            this.lifecycleService = lifecycleService;
            this.currentView = this.backStack.FirstOrDefault();
        }

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

            this.Bridge.SendMessage(
                new BridgeMessage() { FunctionName = "navigateTo", Params = new object[] { pageName } });

            if (!removeBackEntry)
            {
                this.backStack.Add(pageName);
            }

            this.currentView = pageName;
            this.OnNavigatedTo(new NavigationEventHandlerArgs(pageName, NavigationType.Forward, parameter));

            return true;
        }

        public async Task<bool> GoBack()
        {
            //MainPage<TLocator> mainPage = (MainPage<TLocator>)Window.Current.Content;

            if (this.CanGoBack)
            {
                var navigationEventHandlerArgs = new NavigationEventHandlerArgs(this.currentView,
                    NavigationType.Backward,
                    null);
                this.OnNavigatingFrom(navigationEventHandlerArgs);
                Task<NavigationResult>[] tasks = navigationEventHandlerArgs.Tasks.ToArray();

                NavigationResult[] navigationResults = await Task.WhenAll(tasks);

                if (navigationResults.Any(res => res == NavigationResult.Cancel))
                {
                    return false;
                }

                this.backStack.RemoveAt(this.backStack.Count - 1);
                var viewName = this.backStack[this.backStack.Count - 1];

                this.Bridge.SendMessage(
                    new BridgeMessage() { FunctionName = "navigateTo", Params = new object[] { viewName } });

                this.currentView = viewName;

                if (this.currentView != null)
                {
                    this.OnNavigatedTo(new NavigationEventHandlerArgs(this.currentView, NavigationType.Backward,
                        null));
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
                return this.backStack.Count > 1;
            }
        }

        protected virtual void OnNavigatingFrom(NavigationEventHandlerArgs e)
        {
            NavigationEventHandler handler = this.NavigatingFrom;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnNavigatedTo(NavigationEventHandlerArgs e)
        {
            NavigationEventHandler handler = this.NavigatingTo;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event NavigationEventHandler NavigatingFrom;
        public event NavigationEventHandler NavigatingTo;
    }
}