namespace OpenMVVM.Samples.Basic.WebView.Android
{
    using global::Android.App;
    using global::Android.OS;
    using global::Android.Views;

    using OpenMVVM.WebView.Android;

    [Activity(Label = "OpenMVVM.Samples.Basic.WebView.Android", MainLauncher = true)]
    public class MainActivity : WebViewActivity
    {
        public MainActivity()
            : base(new ViewModelLocator())
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.RequestWindowFeature(WindowFeatures.NoTitle);

            this.SetContentView(Resource.Layout.Main);

            var webView = this.FindViewById<global::Android.Webkit.WebView>(Resource.Id.webView);
            this.InitializeWebView(webView);

        }
    }
}

