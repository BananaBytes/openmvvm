namespace OpenMVVM.Samples.Basic.ViewModel
{
    using OpenMVVM.Samples.Basic.ViewModel.Model;

    public class ItemViewModel
    {
        private readonly Item item;

        public ItemViewModel(Item item)
        {
            this.item = item;
        }

        public string Title => this.item.Title;

        public string ImageUrl => this.item.ImageUrl;

        public string Description => this.item.Description;

        public Item GetModel()
        {
            return this.item;
        }
    }
}
