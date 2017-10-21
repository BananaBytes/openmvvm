namespace OpenMVVM.Samples.Basic.WebView.DotNetCore
{
    using OpenMVVM.WebView;
    using OpenMVVM.WebView.DotNetCore;

    public class MVVMContext
    {
        private const string JsContextName = "OpenMVVM";

        private readonly string id;

        private readonly WebViewApp webViewApp;

        private readonly DotNetCoreBridge bridge;

        private readonly dynamic clientsCaller;

        public MVVMContext(
            string id,
            dynamic clientsCaller,
            WebViewApp webViewApp,
            ViewModelLocator viewModelLocatorBase,
            DotNetCoreBridge bridge)
        {
            this.ViewModelLocatorBase = viewModelLocatorBase;
            this.id = id;
            this.clientsCaller = clientsCaller;
            this.webViewApp = webViewApp;
            this.bridge = bridge;
        }

        public ViewModelLocator ViewModelLocatorBase { get; }

        public WebViewApp ViewApp
        {
            get
            {
                return this.webViewApp;
            }
        }

        public void MessageSent(object sender, BridgeMessage bridgeMessage)
        {
            this.clientsCaller.receiveMessage(JsContextName, bridgeMessage);
        }

        public void FromJs(string message)
        {
            this.bridge.WebViewControlScriptNotify(message);
        }
    }
}