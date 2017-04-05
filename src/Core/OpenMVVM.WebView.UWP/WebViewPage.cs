namespace OpenMVVM.WebView.UWP
{
    using System;

    using global::Windows.UI.Core;
    using global::Windows.UI.Xaml.Controls;
    using global::Windows.UI.Xaml.Navigation;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;

    public class WebViewPage : Page
    {
        private readonly Uri homeUri;

        private readonly WebViewApp webViewApp;

        public event EventHandler AppReady;

        public WebViewPage(ViewModelLocatorBase viewModelLocator)
        {
            this.homeUri = new Uri("ms-appx-web:///www/index.html", UriKind.Absolute);
            var webViewControl = new WebView()
            {
                HorizontalAlignment = global::Windows.UI.Xaml.HorizontalAlignment.Stretch,
                VerticalAlignment = global::Windows.UI.Xaml.VerticalAlignment.Stretch
            };
            
            this.Content = webViewControl;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            var windowsBridge = new WindowsBridge(webViewControl);

            this.webViewApp = new WebViewApp(viewModelLocator, windowsBridge, this.OnAppReady);

            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            ((NavigationService)navigationService).Bridge = windowsBridge;

            webViewControl.Navigate(this.homeUri);

            SystemNavigationManager.GetForCurrentView().BackRequested += this.AppBackRequested;
        }

        protected virtual void OnAppReady()
        {
            this.AppReady?.Invoke(this, EventArgs.Empty);
        }

        private void AppBackRequested(object sender, BackRequestedEventArgs e)
        {
            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            navigationService.GoBack();
            e.Handled = true;
        }
    }
}
