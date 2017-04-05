namespace OpenMVVM.WebView.Ios
{
    using System;

    using cdeutsch;

    using Foundation;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;

    using UIKit;

    public partial class WebViewController : UIViewController
    {
        private readonly ViewModelLocatorBase viewModelLocator;

        protected UIWebView webView;

        private WebViewApp webViewApp;

        public event EventHandler AppReady;

        public WebViewController(IntPtr handle, ViewModelLocatorBase viewModelLocator)
            : base(handle)
        {
            this.viewModelLocator = viewModelLocator;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            JsBridge.EnableJsBridge();

            this.AppReady += this.WebViewControllerAppReady;

            var bridge = new IosBridge(this.webView);
            this.webViewApp = new WebViewApp(this.viewModelLocator, bridge, this.OnAppReady);

            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            ((NavigationService)navigationService).Bridge = bridge;
            
            string path = NSBundle.MainBundle.PathForResource("www/index", "html");

            string address = $"file:{path}".Replace(" ", "%20");

            this.webView.LoadRequest(new NSUrlRequest(new NSUrl(address)));
        }

        protected virtual void OnAppReady()
        {
            this.AppReady?.Invoke(this, EventArgs.Empty);
        }

        private void WebViewControllerAppReady(object sender, EventArgs e)
        {
            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            navigationService.NavigateTo("MainView");
        }
    }
}