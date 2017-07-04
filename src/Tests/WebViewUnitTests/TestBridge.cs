namespace WebViewUnitTests
{
    using System;

    using OpenMVVM.WebView;

    public class TestBridge : Bridge
    {
        public Action<BridgeMessage> Action { get; set; }

        public override void SendMessage(BridgeMessage message)
        {
            this.Action(message);
        }

        public void OnReceiveMessage(BridgeMessage bridgeMessage)
        {
            this.OnMessageReceived(bridgeMessage);
        }
    }
}