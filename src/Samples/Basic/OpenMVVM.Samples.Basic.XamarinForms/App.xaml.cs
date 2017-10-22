namespace OpenMVVM.Samples.Basic.XamarinForms
{
    using System;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.XamarinForms;

    using Xamarin.Forms;

    public partial class App : Application
    {
        public App(Action action)
        {
            this.InitializeComponent();
            
            this.MainPage = new RootNavigationPage(new LandingView());
            action();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>().NavigateTo("MainView");
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
