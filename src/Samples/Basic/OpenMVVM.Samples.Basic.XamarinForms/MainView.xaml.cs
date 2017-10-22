namespace OpenMVVM.Samples.Basic.XamarinForms
{
    using OpenMVVM.Core;
    using OpenMVVM.Samples.Basic.ViewModel;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            this.InitializeComponent();
            this.BindingContext = ViewModelLocatorBase.InstanceFactory.GetInstance<MainViewModel>();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((MainViewModel)this.BindingContext).NavigateToItemCommand.Execute(e.SelectedItem);
        }
    }
}