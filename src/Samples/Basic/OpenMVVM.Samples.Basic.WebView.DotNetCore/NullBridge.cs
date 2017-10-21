namespace OpenMVVM.WebView.Web.PlatformServices
{
    using System;

    public class NullBridge : Bridge
    {
        public EventHandler<BridgeMessage> MessageSent;

        public NullBridge()
        {
            
        }
         
        public override void SendMessage(BridgeMessage message)
        {
            this.MessageSent?.Invoke(this, message);
        }

        public void WebViewControlScriptNotify(string message)
        {
            var bridgeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<BridgeMessage>(message);
            this.OnMessageReceived(bridgeMessage);
        }
    }
}
