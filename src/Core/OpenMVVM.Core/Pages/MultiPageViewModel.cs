namespace OpenMVVM.Core.Pages
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using OpenMVVM.Core.PlatformServices.Navigation;

    public class MultiPageViewModel<TParam> : PageViewModel<TParam>
    {
        private readonly ObservableCollection<PageItemViewModel<TParam>> pageItems = new ObservableCollection<PageItemViewModel<TParam>>();

        private int currentPageIndex = -1;

        private PageItemViewModel<TParam> currentPage;

        private bool isMasterVisible;

        public MultiPageViewModel(string pageName, INavigationService navigationService) : base(pageName, navigationService)
        {
            this.Title = string.Empty;
            this.IsMasterVisible = false;
        }

        public ObservableCollection<PageItemViewModel<TParam>> PageItems
        {
            get { return this.pageItems; }
        }

        public PageItemViewModel<TParam> CurrentPage
        {
            get
            {
                return this.currentPage;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                this.Set(() => this.CurrentPage, ref this.currentPage, value);
                this.CurrentPageIndex = this.pageItems.IndexOf(this.currentPage);
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                return this.currentPageIndex;
            }
            set
            {
                if (value == -1 || value == this.currentPageIndex)
                {
                    return;
                }

                if (this.currentPageIndex != -1)
                {
                    this.PageItems[this.currentPageIndex].OnNavigatedFrom();
                }

                this.Set(() => this.CurrentPageIndex, ref this.currentPageIndex, value);
                this.IsMasterVisible = false;
                Task.Run(() => this.PageItems[this.currentPageIndex].OnNavigatedTo());
            }
        }

        public bool IsMasterVisible
        {
            get
            {
                return this.isMasterVisible;
            }
            set
            {
                this.Set(() => this.IsMasterVisible, ref this.isMasterVisible, value);
            }
        }

        public string Title { get; set; }
    }
}
