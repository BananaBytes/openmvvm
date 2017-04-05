namespace OpenMVVM.WebView
{
    using System;

    public interface IBridge
    {
        event EventHandler<BridgeMessage> MessageReceived;

        void SendMessage(BridgeMessage message);
    }
}
