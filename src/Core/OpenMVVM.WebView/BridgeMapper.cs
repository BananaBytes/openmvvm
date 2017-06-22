namespace OpenMVVM.WebView
{
    using System.Linq;
    using System.Reflection;

    public class BridgeMapper
    {
        private readonly IBridge bridge;

        private readonly object targetObject;

        public BridgeMapper(IBridge bridge, object targetObject)
        {
            this.bridge = bridge;
            this.targetObject = targetObject;

            this.bridge.MessageReceived += this.BridgeMessageReceived;
        }

        public void NotifyValueChanged(string path, object value)
        {
            this.bridge.SendMessage(
                new BridgeMessage() { FunctionName = "setValue", Params = new object[] { path, value } });
        }

        public void NotifyCollectionChanged(string path, object param)
        {
            this.bridge.SendMessage(
                new BridgeMessage() { FunctionName = "handleCollectionChange", Params = new object[] { path, param } });
        }

        private void BridgeMessageReceived(object sender, BridgeMessage e)
        {
            var targetObjectType = this.targetObject.GetType();
            var methodInfo = targetObjectType.GetRuntimeMethods().FirstOrDefault(m => m.Name == e.FunctionName && m.GetParameters().Length == e.Params.Length);

            methodInfo?.Invoke(this.targetObject, e.Params);
        }
    }
}