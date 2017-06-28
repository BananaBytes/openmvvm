namespace OpenMVVM.WebView.WPF
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    public class ScriptingHelper
    {
        private readonly Action<string> webViewControlScriptNotify;

        public ScriptingHelper(Action<string> webViewControlScriptNotify)
        {
            this.webViewControlScriptNotify = webViewControlScriptNotify;
        }

        public void Notify(string message)
        {
            this.webViewControlScriptNotify(message);
        }
    }
}
