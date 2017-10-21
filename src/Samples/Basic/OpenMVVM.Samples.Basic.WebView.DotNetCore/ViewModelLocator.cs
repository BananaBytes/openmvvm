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
        //private ILifecycleService lifecycleService = new LifecycleService();
        //private IDescriptionService descriptionService = new NullDescriptionService();

        //private INavigationService navigationService;

        //public static IBridge Bridge = new NullBridge();

        //private readonly MainViewModel mainViewModel;

        //private readonly DetailViewModel detailViewModel;

        public ViewModelLocator()
        {
            var ioc = IocInstanceFactory.DefaultWeb;

            // Services
            ioc.RegisterType<ILifecycleService, LifecycleService>();
            ioc.RegisterType<INavigationService, NavigationService>();
            ioc.RegisterType<IDispatcherService, NullDispatcherService>();
            ioc.RegisterType<IDescriptionService, NullDescriptionService>();

            // ViewModels
            ioc.RegisterType<MainViewModel>();
            ioc.RegisterType<DetailViewModel>();
        }

        public MainViewModel MainViewModel => InstanceFactory.GetInstance<MainViewModel>();

        public DetailViewModel DetailViewModel => InstanceFactory.GetInstance<DetailViewModel>();

        public INavigationService NavigationService => InstanceFactory.GetInstance<INavigationService>();
    }
}