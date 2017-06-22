namespace OpenMVVM.XamarinForms.Ios
{
    using System;
    using System.Linq;

    using OpenMVVM.Core;
    using OpenMVVM.Core.PlatformServices.Lifecycle;

    using UIKit;

    using Xamarin.Forms;

    public class IosNavigationService : NavigationService
    {
        public IosNavigationService(RootNavigationPage navigationPage, ILifecycleService lifecycleService)
            : base(navigationPage, lifecycleService)
        {

        }

        private void Handler(object sender, EventArgs eventArgs)
        {
            this.GoBack();
        }

        private void MenuHandler(object sender, EventArgs eventArgs)
        {
            this.GoBack();
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        protected override void PostPush(Page page)
        {
            var platformViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            var rootViewController = platformViewController.ChildViewControllers.First();

            var currentPage = rootViewController.ChildViewControllers.FirstOrDefault(p => GetPropValue(p, "Child") == page);

            if (this.NavigationPage.Navigation.NavigationStack.Count == 1)
            {
                if (currentPage != null)
                {
                    if (currentPage.NavigationItem.LeftBarButtonItem == null)
                    {
                        UIBarButtonItem menuButton = new UIBarButtonItem(
                            UIImage.FromBundle("ic_drawer.png"),
                            UIBarButtonItemStyle.Plain,
                            (e, a) =>
                                {
                                    ((MasterDetailPage)page).IsPresented = !((MasterDetailPage)page).IsPresented;
                                });
                        currentPage.NavigationItem.SetLeftBarButtonItem(menuButton, false);

                        Xamarin.Forms.NavigationPage.SetHasBackButton(page, true);
                    }
                }
            }
            else
            {

                if (currentPage != null)
                {
                    if (currentPage.NavigationItem.LeftBarButtonItem == null)
                    {
                        UIBarButtonItem uiBarButtonItem = new UIBarButtonItem("< Back", UIBarButtonItemStyle.Plain, this.Handler);
                        currentPage.NavigationItem.SetLeftBarButtonItem(uiBarButtonItem, false);

                        Xamarin.Forms.NavigationPage.SetHasBackButton(page, true);
                    }
                }
            }
        }

        protected override void PrePush(Page page, bool isRoot)
        {
            Xamarin.Forms.NavigationPage.SetHasBackButton(page, !isRoot);
        }
    }
}
