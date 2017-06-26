namespace OpenMVVM.XamarinForms
{
    using System;

    using Xamarin.Forms;

    public class RootNavigationPage : NavigationPage
    {
        public RootNavigationPage(Page firstView) : base(firstView)
        {

        }

        public RootNavigationPage()
            : base()
        {

        }

        public event EventHandler BackButtonPressedEvent;

        protected override bool OnBackButtonPressed()
        {
            this.OnBackButtonPressedEvent();
            return true;
        }

        protected void OnBackButtonPressedEvent()
        {
            this.BackButtonPressedEvent?.Invoke(this, null);
        }
    }
}