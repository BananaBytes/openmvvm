namespace OpenMVVM.WebView.UWP
{
    using global::Windows.ApplicationModel.Core;
    using global::Windows.UI.Core;
    using global::Windows.UI.Xaml.Controls;

    using Newtonsoft.Json;

    public class WindowsBridge : Bridge
    {
        private const string JsContextName = "OpenMVVM";
        private const string JsFunction = "receiveMessage";

        private WebView webViewControl;

        public WindowsBridge(WebView webViewControl)
        {
            this.webViewControl = webViewControl;
            this.webViewControl.ScriptNotify += this.WebViewControlScriptNotify;
        }

        public override void SendMessage(BridgeMessage message)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                    {
                        this.webViewControl.InvokeScriptAsync(
                            JsFunction,
                            new[] { JsContextName, JsonConvert.SerializeObject(message) });
                    });
        }

        private void WebViewControlScriptNotify(object sender, NotifyEventArgs e)
        {
            var bridgeMessage = JsonConvert.DeserializeObject<BridgeMessage>(e.Value);
            this.OnMessageReceived(bridgeMessage);
        }
    }
}
