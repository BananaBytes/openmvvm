namespace HelloWorld.App.DotNetCore
{
    using OpenMVVM.Samples.Basic.WebView.DotNetCore;
    using OpenMVVM.WebView;

    public class MVVMContext
    {
        public ViewModelLocator ViewModelLocatorBase { get; }

        private readonly string id;

        //private readonly Action<dynamic, string> notify;

        //private readonly dynamic clientsCaller;

        private readonly WebViewApp webViewApp;

        public MVVMContext(
            string id,
            WebViewApp webViewApp,
            ViewModelLocator viewModelLocatorBase
            //Action<dynamic, string> notify, 
                           //dynamic clientsCaller, 
        )
        {
            this.ViewModelLocatorBase = viewModelLocatorBase;
            this.id = id;
            // this.notify = notify;
            // this.clientsCaller = clientsCaller;
            this.webViewApp = webViewApp;
        }

        public WebViewApp ViewApp
        {
            get
            {
                return this.webViewApp;
            }
        }
    }
}