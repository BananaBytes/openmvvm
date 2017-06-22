namespace OpenMVVM.WebView
{
    using System;

    public abstract class Bridge : IBridge
    {
        public event EventHandler<BridgeMessage> MessageReceived;

        public abstract void SendMessage(BridgeMessage message);

        protected virtual void OnMessageReceived(BridgeMessage e)
        {
            this.MessageReceived?.Invoke(this, e);
        }
    }
}