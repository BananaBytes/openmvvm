namespace OpenMVVM.Samples.Basic.WebView.DotNetCore
{
    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Samples.Basic.ViewModel;
    using OpenMVVM.WebView;
    using OpenMVVM.WebView.Web.PlatformServices;

    public class ViewModelLocator : ViewModelLocatorBase
    {
        private ILifecycleService lifecycleService = new LifecycleService();
        private IDescriptionService descriptionService = new NullDescriptionService();

        private INavigationService navigationService;

        public static IBridge Bridge = new NullBridge();

        private readonly MainViewModel mainViewModel;

        private readonly DetailViewModel detailViewModel;

        public ViewModelLocator()
        {
            this.navigationService = new NavigationService(this.lifecycleService);

            this.mainViewModel = new MainViewModel(this.NavigationService, this.descriptionService);
            this.detailViewModel = new DetailViewModel(this.NavigationService);

            //var ioc = InstanceFactory;

            //// Infrastructure
            //ioc.RegisterType<IBridge, WindowsBridge>();

            //// Services
            //ioc.RegisterType<ILifecycleService, LifecycleService>();
            //ioc.RegisterType<INavigationService, NavigationService<ViewModelLocator>>();
            //ioc.RegisterType<ICacheService, CacheService>();
            //ioc.RegisterType<IContentDialogService, ContentDialogService>();
            //ioc.RegisterType<IDispatcherService, DispatcherService>();
            //ioc.RegisterType<IDescriptionService, DescriptionService>();

            //// ViewModels
            //ioc.RegisterType<MainViewModel>();
            //ioc.RegisterType<DetailViewModel>();



        }

        public MainViewModel MainViewModel
        {
            get
            {
                return this.mainViewModel;
            }
        }

        public DetailViewModel DetailViewModel
        {
            get
            {
                return this.detailViewModel;
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return this.navigationService;
            }
        }
    }
}