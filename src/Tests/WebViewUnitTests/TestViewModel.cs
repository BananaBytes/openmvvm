namespace WebViewUnitTests
{
    using OpenMVVM.Core;

    public class TestViewModel : ObservableObject
    {
        private string title;

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
    }
}