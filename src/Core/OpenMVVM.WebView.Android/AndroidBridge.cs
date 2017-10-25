namespace OpenMVVM.WebView.Android
{
    using global::Android.App;

    using Newtonsoft.Json;

    public class AndroidBridge : Bridge
    {
        private const string JsContextName = "OpenMVVM";
        private const string JsFunction = "receiveMessage";

        private global::Android.Webkit.WebView webViewControl;

        public AndroidBridge(global::Android.Webkit.WebView webViewControl, Activity activity)
        {
            this.webViewControl = webViewControl;

            var javaScriptMessageHandler =
                new JavaScriptMessageHandler(activity) { NotifyAction = this.WebViewControlScriptNotify };
            webViewControl.AddJavascriptInterface(javaScriptMessageHandler, "NotifyCs");
        }

        public override void SendMessage(BridgeMessage message)
        {
            this.webViewControl.LoadUrl("javascript:" + JsFunction + "('" + JsContextName + "', '" + JsonConvert.SerializeObject(message).Replace("'", "\\'") + "');");
        }

        private void WebViewControlScriptNotify(string message)
        {
            var bridgeMessage = JsonConvert.DeserializeObject<BridgeMessage>(message);
            this.OnMessageReceived(bridgeMessage);
        }
    }
}
