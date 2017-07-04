namespace WebViewUnitTests
{
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using OpenMVVM.WebView;

    [TestClass]
    public class WebViewBridgeUnitTest
    {
        private TestBridge testBridge;

        private TestViewModelLocator testViewModelLocator;

        private WebViewApp webViewApp;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testBridge = new TestBridge();
            this.testViewModelLocator = new TestViewModelLocator();
            this.webViewApp = new WebViewApp(this.testViewModelLocator, this.testBridge, () => { });
        }

        [TestMethod]
        public async Task PropertyChangedMessageTest()
        {
            bool called = false;

            this.testBridge.OnReceiveMessage(
                new BridgeMessage()
                    {
                        FunctionName = "RegisterBinding",
                        Params = new object[] { "TestViewModel.Title", false }
                    });

            var newText = "Some text";
            this.testBridge.Action = (m) =>
                {
                    Assert.AreEqual(m.FunctionName, "setValue", "Wrong function called.");
                    Assert.AreEqual(m.Params.Length, 2, "Wrong parameter number supplied.");

                    Assert.AreEqual((string)m.Params[0], "TestViewModel.Title", "Message contains wrong prop path.");
                    Assert.AreEqual((string)m.Params[1], newText, "Message contains wrong prop value.");

                    called = true;
                };

            this.testViewModelLocator.TestViewModel.Title = newText;

            await Task.Delay(1000);
            Assert.IsTrue(called, "Message not sent.");
        }
    }
}
