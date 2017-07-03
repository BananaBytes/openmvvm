namespace WebViewUnitTests
{
    using OpenMVVM.Core;

    public class TestViewModelLocator : ViewModelLocatorBase
    {
        public TestViewModelLocator()
        {
            var ioc = IocInstanceFactory.Default;

            ioc.RegisterType<TestViewModel>();
        }

        public TestViewModel TestViewModel
        {
            get
            {
                return IocInstanceFactory.Default.GetInstance<TestViewModel>();
            }
        }
    }
}