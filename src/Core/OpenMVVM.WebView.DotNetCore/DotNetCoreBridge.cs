namespace OpenMVVM.WebView.DotNetCore
{
    using System;

    public class DotNetCoreBridge : Bridge
    {
        public EventHandler<BridgeMessage> MessageSent;

        public DotNetCoreBridge()
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
