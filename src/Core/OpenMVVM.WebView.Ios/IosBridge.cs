namespace OpenMVVM.WebView.Ios
{
    using cdeutsch;

    using Newtonsoft.Json;

    using UIKit;

    public class IosBridge : Bridge
    {
        private const string JsContextName = "OpenMVVM";
        private const string JsFunction = "receiveMessage";

        private readonly UIWebView webViewControl;

        public IosBridge(UIWebView webViewControl)
        {
            this.webViewControl = webViewControl;
            this.webViewControl.AddEventListener("doNativeStuff", this.WebViewControlScriptNotify);
        }

        public override void SendMessage(BridgeMessage message)
        {
            this.webViewControl.FireEvent(JsFunction, new[] { JsContextName, JsonConvert.SerializeObject(message) });
        }

        private void WebViewControlScriptNotify(FireEventData message)
        {
            var bridgeMessage = JsonConvert.DeserializeObject<BridgeMessage>((string)message.JsonData);
            this.OnMessageReceived(bridgeMessage);
        }
    }
}
