namespace OpenMVVM.Samples.Basic.XamarinForms.Droid
{
    using global::Android.App;
    using global::Android.Content.PM;
    using global::Android.OS;

    using OpenMVVM.Samples.Basic.XamarinForms;

    [Activity(Label = "HelloWorld.App.XamarinForms", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            
            LoadApplication(new App(()=>{ var vmLocator = new ViewModelLocator(); }));
        }
    }
}

