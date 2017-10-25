namespace OpenMVVM.Samples.Basic.XamarinForms.iOS
{
    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Lifecycle;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Ios.PlatformServices;
    using OpenMVVM.Samples.Basic.ViewModel;
    using OpenMVVM.Samples.Basic.ViewModel.Services;
    using OpenMVVM.XamarinForms.Ios;

    public class ViewModelLocator : ViewModelLocatorBase
    {
        public ViewModelLocator()
        {
            var ioc = IocInstanceFactory.Default;
            
            // Services
            ioc.RegisterType<ILifecycleService, IosLifecycleService>();
            ioc.RegisterType<INavigationService, IosNavigationService>();
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
