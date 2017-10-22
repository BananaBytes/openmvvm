namespace OpenMVVM.Samples.Basic.XamarinForms
{
    using OpenMVVM.Core;
    using OpenMVVM.Samples.Basic.ViewModel;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailView : ContentPage
    {
        public DetailView()
        {
            this.InitializeComponent();
            this.BindingContext = ViewModelLocatorBase.InstanceFactory.GetInstance<DetailViewModel>();
        }
    }
}