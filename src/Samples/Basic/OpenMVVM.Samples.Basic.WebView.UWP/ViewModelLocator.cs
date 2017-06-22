namespace OpenMVVM.Samples.Basic.WebView.UWP
{
    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Samples.Basic.ViewModel;
    using OpenMVVM.UWP.PlatformServices;
    using OpenMVVM.WebView;
    using OpenMVVM.WebView.UWP;

    public class ViewModelLocator : ViewModelLocatorBase
    {
        public ViewModelLocator()
        {
            var ioc = IocInstanceFactory.DefaultWeb;

            // Infrastructure
            ioc.RegisterType<IBridge, WindowsBridge>();

            // Services
            ioc.RegisterType<ILifecycleService, LifecycleService>();
            ioc.RegisterType<INavigationService, NavigationService>();
            ioc.RegisterType<ICacheService, CacheService>();
            ioc.RegisterType<IContentDialogService, ContentDialogService>();
            ioc.RegisterType<IDispatcherService, DispatcherService>();
            ioc.RegisterType<IDescriptionService, DescriptionService>();

            // ViewModels
            ioc.RegisterType<MainViewModel>();
            ioc.RegisterType<DetailViewModel>();
        }

        public MainViewModel MainViewModel => InstanceFactory.GetInstance<MainViewModel>();

        public DetailViewModel DetailViewModel => InstanceFactory.GetInstance<DetailViewModel>();

    }
}
