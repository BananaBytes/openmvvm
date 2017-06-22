namespace OpenMVVM.Samples.Basic.XamarinForms.Droid
{
    using OpenMVVM.Android.PlatformServices;
    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Samples.Basic.ViewModel;
    using OpenMVVM.Samples.Basic.XamarinForms;
    using OpenMVVM.XamarinForms;

    public class ViewModelLocator : ViewModelLocatorBase
    {
        public ViewModelLocator()
        {
            var ioc = IocInstanceFactory.Default;
            
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

            ioc.RegisterInstanceByKey<MainView>("MainView");
            ioc.RegisterInstanceByKey<DetailView>("DetailView");
        }

        public MainViewModel MainViewModel => InstanceFactory.GetInstance<MainViewModel>();

        public DetailViewModel DetailViewModel => InstanceFactory.GetInstance<DetailViewModel>();

    }
}
