namespace OpenMVVM.WebView.WPF
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Controls;

    using Newtonsoft.Json;

    using OpenMVVM.WPF.PlatformServices;

    public class WpfBridge : Bridge
    {
        private const string JsContextName = "OpenMVVM";
        private const string JsFunction = "receiveMessage";

        private WebBrowser webViewControl;

        private DispatcherService dispatcherService;

        public WpfBridge(WebBrowser webViewControl)
        {
            this.dispatcherService = new DispatcherService();
            this.webViewControl = webViewControl;
            this.webViewControl.ObjectForScripting = new ScriptingHelper(this.WebViewControlScriptNotify);
        }

        public override void SendMessage(BridgeMessage message)
        {
            this.dispatcherService.RunAsync(
                () =>
                    {
                        this.webViewControl.InvokeScript(
                            JsFunction,
                            new[] { JsContextName, JsonConvert.SerializeObject(message) });
                    });
        }

        private void WebViewControlScriptNotify(string message)
        {
            var bridgeMessage = JsonConvert.DeserializeObject<BridgeMessage>(message);
            this.OnMessageReceived(bridgeMessage);
        }

    }
}
