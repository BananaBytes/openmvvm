namespace HelloWorld.App.DotNetCore
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.SignalR;

    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Samples.Basic.WebView.DotNetCore;
    using OpenMVVM.WebView;
    using OpenMVVM.WebView.Web.PlatformServices;

    public class OpenMvvmHub : Hub
    {
        private const string JsContextName = "OpenMVVM";
        private const string JsFunction = "receiveMessage";

        public static Dictionary<string, MVVMContext> contexts = new Dictionary<string, MVVMContext>();

        private static NullBridge bridge = new NullBridge();

        private static bool init = false;

        public OpenMvvmHub()
        {

            if (!init)
            {
                init = true;

                bridge.MessageSent += this.MessageSent;
            }
        }

        public void Register(string id)
        {
            if (!contexts.ContainsKey(id))
            {
                var viewModelLocatorBase = new ViewModelLocator();
                ((NavigationService)viewModelLocatorBase.NavigationService).Bridge = bridge;
                contexts.Add(
                    id,
                    new MVVMContext(
                        id,
                        //new Action<dynamic, string>(this.Notify),
                        //this.Clients.Caller,
                        new WebViewApp(viewModelLocatorBase, bridge, this.OnAppReady), viewModelLocatorBase));
                

            }
        }

        private void OnAppReady()
        {
            contexts.First().Value.ViewModelLocatorBase.NavigationService.NavigateTo("MainView");
        }

        public void MessageSent(object sender, BridgeMessage bridgeMessage)
        {
            this.Clients.All.receiveMessage(JsContextName, bridgeMessage);
        }

        public void FromJs(string message)
        {
            bridge.WebViewControlScriptNotify(message);
        }
    }
}