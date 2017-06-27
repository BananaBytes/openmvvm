namespace OpenMVVM.Samples.Basic.WebView.UWP
{
    using System;
    using System.Diagnostics;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.Foundation.Metadata;
    using Windows.UI;
    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.WebView.UWP;

    internal sealed partial class App : Application
    {
        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            WebViewPage webViewPage = new WebViewPage(new ViewModelLocator());
            webViewPage.AppReady += this.CurrentContentAppReady;

            Window.Current.Content = webViewPage;
            Window.Current.Activate();

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Colors.Black;
                    statusBar.ForegroundColor = Colors.White;
                }
            }
        }

        private void CurrentContentAppReady(object sender, EventArgs e)
        {
            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            navigationService.NavigateTo("MainView");
        }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}