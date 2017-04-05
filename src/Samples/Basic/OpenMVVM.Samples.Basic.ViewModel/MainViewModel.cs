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

    public class MainViewModel : PageViewModel<object>
    {
        private readonly List<ItemViewModel> itemList = new List<ItemViewModel>();

        private ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();

        private bool initialized;

        private string searchInput;

        private IMvvmCommand navigateToItemCommand;

        public MainViewModel(INavigationService navigationService, IDescriptionService descriptionService)
            : base("MainView", navigationService)
        {
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
                    .Where(i => i.Title.ToLower().Contains(value.ToLower()) || i.Description.ToLower().Contains(value.ToLower()))
                    .ToList();

                this.Items.Clear();

                foreach (var item in filteredList)
                {
                    this.Items.Add(item);
                }
            }
        }

        protected override void OnNavigatedTo(object parameter)
        {
            base.OnNavigatedTo(parameter);

            if (!this.initialized)
            {
                this.itemList.Add(new ItemViewModel(new Item() { Title = "Antoine", Description = "Sed ut" }));
                this.itemList.Add(new ItemViewModel(new Item() { Title = "Temple", Description = "Perspiciatis unde" }));
                this.itemList.Add(new ItemViewModel(new Item() { Title = "Sarah", Description = "Ut enim" }));
                this.itemList.Add(new ItemViewModel(new Item() { Title = "Maira", Description = "Minima veniam" }));
                this.itemList.Add(new ItemViewModel(new Item() { Title = "Louie", Description = "Sequi nesciunt" }));
                this.itemList.Add(new ItemViewModel(new Item() { Title = "John", Description = "Totam rem" }));
                this.itemList.Add(new ItemViewModel(new Item() { Title = "Ardella", Description = "Nemo enim" }));

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
