namespace OpenMVVM.Samples.Basic.ViewModel
{
    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;
    using OpenMVVM.Core.PlatformServices.Navigation;

    public class DetailViewModel : PageViewModel<ItemViewModel>
    {
        private string title;

        private IMvvmCommand goBackCommand;

        public DetailViewModel(INavigationService navigationService)
            : base("DetailView", navigationService)
        {
        }

        public IMvvmCommand GoBackCommand => this.goBackCommand ?? (this.goBackCommand = new ActionCommand(() =>
            {
                this.NavigationService.GoBack();
            }));

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.Set(ref this.title, value);
            }
        }

        protected override void OnNavigatedTo(ItemViewModel item)
        {
            base.OnNavigatedTo(item);

            this.Title = $"Hello, {item.Title}!";
        }
    }
}
