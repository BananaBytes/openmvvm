namespace OpenMVVM.Core.Pages
{
    using System.Windows.Input;

    public class PageItemViewModel<TParam> : ObservableObject
    {
        protected readonly MultiPageViewModel<TParam> ParentMultiPage;

        private bool isLoading;

        private ICommand goToPageCommand;

        private bool active;

        public PageItemViewModel(MultiPageViewModel<TParam> parentMultiPage)
        {
            this.ParentMultiPage = parentMultiPage;
        }

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.isLoading = value;
                this.RaisePropertyChanged(() => this.IsLoading);
            }
        }

        public string Title { get; set; }

        public string Icon { get; set; }

        public ICommand GoToPageCommand
        {
            get
            {
                return this.goToPageCommand
                       ?? (this.goToPageCommand = new ActionCommand<PageItemViewModel<TParam>>(
                               page =>
                                   {
                                       this.ParentMultiPage.CurrentPage = page;
                                   }));
            }
        }

        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.active = value;
                this.RaisePropertyChanged();
            }
        }

        public virtual void OnNavigatedTo()
        {
            this.Active = true;
        }

        public virtual void OnNavigatedFrom()
        {
            this.Active = false;
        }
    }
}