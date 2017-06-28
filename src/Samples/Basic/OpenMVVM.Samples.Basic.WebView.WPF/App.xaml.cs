namespace OpenMVVM.Samples.Basic.WebView.WPF
{
    using System;
    using System.Windows;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.WebView.WPF;

    public partial class App : Application
    {

        private bool isActivated = false;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!this.isActivated)
            {
                this.isActivated = true;

                WebViewPage webViewPage = new WebViewPage(new ViewModelLocator());
                webViewPage.AppReady += this.CurrentContentAppReady;
                App.Current.MainWindow.Content = webViewPage;
            }   
        }

        private void CurrentContentAppReady(object sender, EventArgs e)
        {
            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            navigationService.NavigateTo("MainView");
        }
    }
}
