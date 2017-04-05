namespace OpenMVVM.Samples.Basic.WebView.Ios
{
    using System;

    public partial class WebViewController : OpenMVVM.WebView.Ios.WebViewController
    {
        public WebViewController(IntPtr handle)
            : base(handle, new ViewModelLocator())
        {
            
        }

        public override void ViewDidLoad()
        {
            base.webView = this.WebView;
            base.ViewDidLoad();
        }
    }
}

