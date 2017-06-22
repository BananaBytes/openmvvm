// WARNING
//
// This file has been generated automatically to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

namespace OpenMVVM.Samples.Basic.WebView.Ios
{
    using System.CodeDom.Compiler;

    using Foundation;

    [Register ("WebViewController")]
	partial class WebViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIWebView WebView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (this.WebView != null) {
				this.WebView.Dispose ();
				this.WebView = null;
			}
		}
	}
}
