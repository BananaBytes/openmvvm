namespace OpenMVVM.WebView.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.WebView.WPF;

    public class WebViewPage : Page
    {
        private readonly Uri homeUri;

        private readonly WebViewApp webViewApp;

        public event EventHandler AppReady;

        public WebViewPage(ViewModelLocatorBase viewModelLocator)
        {
            this.homeUri = new Uri("pack://siteoforigin:,,,/www/index.html", UriKind.Absolute);
            var webViewControl = new WebBrowser()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            
            this.Content = webViewControl;

            var windowsBridge = new WpfBridge(webViewControl);

            this.webViewApp = new WebViewApp(viewModelLocator, windowsBridge, this.OnAppReady);

            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            ((NavigationService)navigationService).Bridge = windowsBridge;

            webViewControl.Navigate(this.homeUri);
        }

        protected virtual void OnAppReady()
        {
            this.AppReady?.Invoke(this, EventArgs.Empty);
        }
    }
}
