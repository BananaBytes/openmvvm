namespace OpenMVVM.Core.Pages
{
    using System;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices.Navigation;

    public class PageViewModel<TParam> : ObservableObject
    {
        private readonly string pageName;
        private readonly INavigationService navigationService;

        private bool isLoading;

        public PageViewModel(string pageName, INavigationService navigationService)
        {
            this.pageName = pageName;
            this.navigationService = navigationService;
            this.navigationService.NavigatingFrom += this.NavigationServiceNavigatingFrom;
            this.navigationService.NavigatingTo += this.NavigationServiceNavigatingTo;
        }

        public bool IsLoading
        {
            get { return this.isLoading; }
            set { this.Set(() => this.IsLoading, ref this.isLoading, value); }
        }

        public INavigationService NavigationService
        {
            get { return this.navigationService; }
        }

        protected virtual void OnNavigatedTo(TParam parameter)
        {

        }

        protected virtual void OnNavigatedForwardTo(TParam parameter)
        {

        }

        protected virtual void OnNavigatedBackwardTo(TParam parameter)
        {

        }

        protected virtual void OnNavigatingFrom()
        {

        }

        protected virtual Task<NavigationResult> OnNavigatingForwardFromAsync()
        {
            return Task.FromResult(NavigationResult.Continue);
        }

        protected virtual Task<NavigationResult> OnNavigatingBackwardFromAsync()
        {
            return Task.FromResult(NavigationResult.Continue);
        }

        private void NavigationServiceNavigatingTo(object sender, NavigationEventHandlerArgs args)
        {
            if (args.PageView == this.pageName)
            {
                switch (args.NavigationType)
                {
                    case NavigationType.Forward:
                        this.OnNavigatedForwardTo((TParam)args.Parameter);
                        break;
                    case NavigationType.Backward:
                        this.OnNavigatedBackwardTo((TParam)args.Parameter);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                this.OnNavigatedTo((TParam)args.Parameter);
            }
        }

        private void NavigationServiceNavigatingFrom(object sender, NavigationEventHandlerArgs args)
        {
            if (args.PageView == this.pageName)
            {
                switch (args.NavigationType)
                {
                    case NavigationType.Forward:
                        args.Tasks.Add(this.OnNavigatingForwardFromAsync());
                        break;
                    case NavigationType.Backward:
                        args.Tasks.Add(this.OnNavigatingBackwardFromAsync());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                this.OnNavigatingFrom();
            }
        }
    }
}
