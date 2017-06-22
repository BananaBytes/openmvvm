namespace OpenMVVM.WebView.Android
{
    using System;

    using global::Android.App;
    using global::Android.OS;
    using global::Android.Views;
    using global::Android.Webkit;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.WebView;

    public class WebViewActivity : Activity
    {
        private const string homePath = "www/index.html";

        private readonly ViewModelLocatorBase viewModelLocator;

        private WebViewApp webViewApp;

        public event EventHandler AppReady;

        public WebViewActivity(ViewModelLocatorBase viewModelLocator)
        {
            this.viewModelLocator = viewModelLocator;
        }

        protected void InitializeWebView(WebView webView)
        {
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.AllowFileAccessFromFileURLs = true;
            webView.Settings.AllowUniversalAccessFromFileURLs = true;
            //global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);

            this.AppReady += this.CurrentContentAppReady;

            var bridge = new AndroidBridge(webView, this);

            this.webViewApp = new WebViewApp(this.viewModelLocator, bridge, this.OnAppReady);

            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            ((NavigationService)navigationService).Bridge = bridge;

            webView.LoadUrl("file:///android_asset/" + homePath);
        }

        private void CurrentContentAppReady(object sender, System.EventArgs e)
        {
            this.RunOnUiThread(
                () =>
                    {
                        var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
                        navigationService.NavigateTo("MainView");
                    });
        }

        public override void OnBackPressed()
        {
            var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
            navigationService.GoBack();
        }

        protected virtual void OnAppReady()
        {
            this.AppReady?.Invoke(this, EventArgs.Empty);
        }
    }
}

