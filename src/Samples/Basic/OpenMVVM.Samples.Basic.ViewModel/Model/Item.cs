namespace OpenMVVM.Samples.Basic.ViewModel.Model
{
    public class Item
    {
        public const string GravatarUrl = "https://www.gravatar.com/avatar/{0}?s=64&d=identicon&r=PG";

        public string Title { get; set; }

        public string ImageUrl => string.Format(GravatarUrl, Helpers.MD5.GetMd5String(this.Title));

        public string Description { get; set; }
    }
}
