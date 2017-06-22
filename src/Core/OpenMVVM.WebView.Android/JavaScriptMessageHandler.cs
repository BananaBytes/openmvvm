namespace OpenMVVM.WebView.Android
{
    using System;

    using global::Android.Content;
    using global::Android.Webkit;

    using Java.Interop;

    public class JavaScriptMessageHandler : Java.Lang.Object
    {
        Context context;

        public JavaScriptMessageHandler(Context context)
        {
            this.context = context;
        }

        public Action<string> NotifyAction { get; set; }

        [Export("notify")]
        [JavascriptInterface]
        public void Notify(string url)
        {
                this.NotifyAction(url);
        }
    }
}