namespace OpenMVVM.Samples.Basic.XamarinForms.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.LoadApplication(new XamarinForms.App(() => { var vmLocator = new ViewModelLocator(); }));
        }
    }
}
