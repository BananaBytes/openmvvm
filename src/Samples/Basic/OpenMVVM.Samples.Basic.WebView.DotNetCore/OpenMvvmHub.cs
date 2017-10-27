namespace OpenMVVM.Samples.Basic.WebView.DotNetCore
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.SignalR;

    using OpenMVVM.WebView;
    using OpenMVVM.WebView.DotNetCore;

    public class OpenMvvmHub : Hub
    {
        private const string JsFunction = "receiveMessage";

        private static Dictionary<string, MVVMContext> contexts = new Dictionary<string, MVVMContext>();

        public void Register(string id)
        {
            if (!contexts.ContainsKey(id))
            {
                var viewModelLocatorBase = new ViewModelLocator();
                DotNetCoreBridge bridge = new DotNetCoreBridge();
                ((NavigationService)viewModelLocatorBase.NavigationService).Bridge = bridge;
                var mvvmContext = new MVVMContext(
                    id,
                    this.Clients.Caller,
                    new WebViewApp(viewModelLocatorBase, bridge, this.OnAppReady),
                    viewModelLocatorBase,
                    bridge);

                bridge.MessageSent += mvvmContext.MessageSent;

                contexts.Add(
                    id,
                    mvvmContext);
            }
        }

        public void FromJs(string id, string message)
        {
            contexts.First(c => c.Key == id).Value.FromJs(message);
        }

        private void OnAppReady()
        {
            contexts.First().Value.ViewModelLocatorBase.NavigationService.NavigateTo("MainView");
        }
    }
}