namespace OpenMVVM.XamarinForms
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;

    using Xamarin.Forms;

    public class NavigationService : INavigationService
    {
        protected readonly RootNavigationPage NavigationPage;
        private readonly IInstanceFactory instanceFactory;
        private readonly ILifecycleService lifecycleService;

        private string currentViewName;

        private bool isNavigating;

        public NavigationService(RootNavigationPage navigationPage, ILifecycleService lifecycleService)
        {
            this.NavigationPage = Application.Current.MainPage as RootNavigationPage;
            this.instanceFactory = IocInstanceFactory.Default;
            this.lifecycleService = lifecycleService;

            this.NavigationPage.BackButtonPressedEvent += this.NavigationPageBackButtonPressedEvent;
        }

        private void NavigationPageBackButtonPressedEvent(object sender, EventArgs e)
        {
            this.GoBack();
        }

        public async Task<bool> NavigateTo(string viewName, object parameter = null, bool removeBackEntry = false)
        {
            if (this.isNavigating)
            {
                return false;
            }
            else
            {
                this.isNavigating = true;
            }

            Page desiredPage = this.instanceFactory.GetInstanceByKey<Page>(viewName);

            Page pageToRemove = null;

            if (desiredPage != null)
            {
                if (!string.IsNullOrEmpty(this.currentViewName))
                {
                    var navigationEventHandlerArgs = new NavigationEventHandlerArgs(this.currentViewName, NavigationType.Forward, parameter);
                    this.OnNavigatingFrom(navigationEventHandlerArgs);
                    Task<NavigationResult>[] tasks = navigationEventHandlerArgs.Tasks.ToArray();

                    NavigationResult[] navigationResults = await Task.WhenAll(tasks);

                    if (navigationResults.Any(res => res == NavigationResult.Cancel))
                    {
                        this.isNavigating = false;
                        return false;
                    }
                }
                else
                {
                    if (this.NavigationPage.Navigation.NavigationStack.Count == 1)
                    {
                        pageToRemove = this.NavigationPage.CurrentPage;
                    }
                }

                this.PrePush(desiredPage, false);
                await this.NavigationPage.Navigation.PushAsync(desiredPage);
                if (pageToRemove != null)
                {
                    this.NavigationPage.Navigation.RemovePage(pageToRemove);
                }
                this.PostPush(desiredPage);

                this.currentViewName = viewName;
                this.OnNavigatingTo(new NavigationEventHandlerArgs(viewName, NavigationType.Forward, parameter));

                this.isNavigating = false;
                return true;
            }

            this.isNavigating = false;
            return false;
        }

        protected virtual void PrePush(Page page, bool isRoot)
        {

        }

        protected virtual void PostPush(Page page)
        {

        }

        public bool CanGoBack { get; }

        public async Task<bool> GoBack()
        {
            var navigationEventHandlerArgs = new NavigationEventHandlerArgs(this.currentViewName, NavigationType.Backward, null);
            OnNavigatingFrom(navigationEventHandlerArgs);
            Task<NavigationResult>[] tasks = navigationEventHandlerArgs.Tasks.ToArray();

            NavigationResult[] navigationResults = await Task.WhenAll(tasks);

            if (navigationResults.Any(res => res == NavigationResult.Cancel))
            {
                return false;
            }

            var poppedView = await NavigationPage.Navigation.PopAsync();
            this.currentViewName = NavigationPage.CurrentPage.GetType().Name;

            // If nothing is popped, we're at the main page, need to exit
            if (poppedView == null)
            {
                if (!lifecycleService.TryCloseApplication())
                {
                    
                }
            }
            else if (this.currentViewName != null)
            {
                this.OnNavigatingTo(new NavigationEventHandlerArgs(currentViewName, NavigationType.Backward, null));
            }

            return true;
        }

        protected virtual void OnNavigatingFrom(NavigationEventHandlerArgs e)
        {
            NavigationEventHandler handler = NavigatingFrom;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnNavigatingTo(NavigationEventHandlerArgs e)
        {
            NavigationEventHandler handler = NavigatingTo;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public string CurrentPage => this.currentViewName;

        public event NavigationEventHandler NavigatingFrom;
        public event NavigationEventHandler NavigatingTo;
    }
}
