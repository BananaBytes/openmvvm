namespace OpenMVVM.WebView
{
    public class BridgeMessage
    {
        public string FunctionName { get; set; }

        public object[] Params { get; set; }
    }
}