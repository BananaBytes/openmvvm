namespace OpenMVVM.WebView.Android
{
    using System.Text;

    using global::Android.App;

    using Newtonsoft.Json;

    public class AndroidBridge : Bridge
    {
        private const string JsContextName = "OpenMVVM";
        private const string JsFunction = "receiveMessage";

        private global::Android.Webkit.WebView webViewControl;

        private readonly Activity activity;

        public AndroidBridge(global::Android.Webkit.WebView webViewControl, Activity activity)
        {
            this.webViewControl = webViewControl;
            this.activity = activity;

            var javaScriptMessageHandler =
                new JavaScriptMessageHandler(activity) { NotifyAction = this.WebViewControlScriptNotify };
            webViewControl.AddJavascriptInterface(javaScriptMessageHandler, "NotifyCs");
        }

        public override void SendMessage(BridgeMessage message)
        {
            var msg = JsonConvert.SerializeObject(message);

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(msg);
            string encoded = System.Convert.ToBase64String(plainTextBytes);
            this.activity.RunOnUiThread(
                () =>
                    {
                        this.webViewControl.LoadUrl("javascript:" + JsFunction + "('" + JsContextName + "', '" + encoded + "');");
                    });
        }

        private void WebViewControlScriptNotify(string message)
        {
            var bridgeMessage = JsonConvert.DeserializeObject<BridgeMessage>(message);
            this.OnMessageReceived(bridgeMessage);
        }
    }
}
