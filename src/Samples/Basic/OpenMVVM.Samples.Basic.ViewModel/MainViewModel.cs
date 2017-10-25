namespace OpenMVVM.Samples.Basic.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.Samples.Basic.ViewModel.Model;
    using OpenMVVM.Samples.Basic.ViewModel.Services;

    public class MainViewModel : PageViewModel<object>
    {
        private readonly IDataService dataService;

        private readonly List<ItemViewModel> itemList = new List<ItemViewModel>();

        private ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();

        private bool initialized;

        private string searchInput;

        private IMvvmCommand navigateToItemCommand;

        public MainViewModel(INavigationService navigationService, IDescriptionService descriptionService, IDataService dataService)
            : base("MainView", navigationService)
        {
            this.dataService = dataService;

            this.Title = $"Hello, {descriptionService.Platform}!";
        }

        public IMvvmCommand NavigateToItemCommand => this.navigateToItemCommand ?? (this.navigateToItemCommand = new ActionCommand<ItemViewModel>(this.NavigateToItem));

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.Set(ref this.items, value);
            }
        }

        public string Title { get; }

        public string SearchInput
        {
            get
            {
                return this.searchInput;
            }

            set
            {
                this.Set(ref this.searchInput, value);

                var filteredList = 
                    this.itemList
                    .Where(i => (i.Title?.ToLower().Contains(value.ToLower()) ?? false) || (i.Description?.ToLower().Contains(value.ToLower()) ?? false))
                    .ToList();

                this.Items.Clear();

                foreach (var item in filteredList)
                {
                    this.Items.Add(item);
                }
            }
        }

        protected override async void OnNavigatedTo(object parameter)
        {
            base.OnNavigatedTo(parameter);

            if (!this.initialized)
            {
                var data = await this.dataService.GetRepositoriesAsync();

                var repositories = data as IList<Repository> ?? data.ToList();
                if (repositories?.Count() > 0)
                {
                    foreach (var item in repositories)
                    {
                        this.itemList.Add(new ItemViewModel(new Item() { Title = item.Name, Description = item.Description, ImageUrl = item.Owner.AvatarUrl }));
                    }
                }

                this.Items = new ObservableCollection<ItemViewModel>(this.itemList);

                this.initialized = true;
            }
        }

        private void NavigateToItem(ItemViewModel item)
        {
            this.NavigationService.NavigateTo("DetailView", item);
        }
    }
}
