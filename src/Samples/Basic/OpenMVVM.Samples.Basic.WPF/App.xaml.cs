namespace OpenMVVM.Samples.Basic.WPF
{
    using System.Windows;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool initialized;

        public App()
        {
            this.Activated += this.AppActivated;
        }

        private void AppActivated(object sender, System.EventArgs e)
        {
            if (!this.initialized)
            {
                this.initialized = true;

                this.Resources.Add("Locator", new ViewModelLocator());

                var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();

                navigationService.NavigateTo("MainView");
            }
        }
    }
}
