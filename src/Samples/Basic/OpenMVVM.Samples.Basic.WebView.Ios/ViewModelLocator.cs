namespace OpenMVVM.Samples.Basic.WebView.Ios
{
    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Ios.PlatformServices;
    using OpenMVVM.Samples.Basic.ViewModel;
    using OpenMVVM.WebView;

    public class ViewModelLocator : ViewModelLocatorBase
    {
        public ViewModelLocator()
        {
            var ioc = IocInstanceFactory.DefaultWeb;

            // Services
            ioc.RegisterType<ILifecycleService, IosLifecycleService>();
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
