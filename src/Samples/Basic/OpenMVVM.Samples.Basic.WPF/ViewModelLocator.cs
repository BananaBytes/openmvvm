namespace OpenMVVM.Samples.Basic.WPF
{
    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Samples.Basic.ViewModel;
    using OpenMVVM.Samples.Basic.ViewModel.Services;
    using OpenMVVM.WPF;
    using OpenMVVM.WPF.PlatformServices;

    public class ViewModelLocator : ViewModelLocatorBase
    {
        public ViewModelLocator()
        {
            var ioc = IocInstanceFactory.Default;

            // Services
            ioc.RegisterType<ILifecycleService, LifecycleService>();
            ioc.RegisterType<INavigationService, NavigationService>();
            ioc.RegisterType<IContentDialogService, ContentDialogService>();
            ioc.RegisterType<IDispatcherService, DispatcherService>();
            ioc.RegisterType<IDescriptionService, DescriptionService>();
            ioc.RegisterType<IDataService, DataService>();

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

