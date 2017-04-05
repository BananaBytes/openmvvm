namespace OpenMVVM.Core.PlatformServices.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NavigationEventHandlerArgs : EventArgs
    {
        public NavigationEventHandlerArgs(string pageView, NavigationType navigationType, dynamic parameter)
        {
            this.PageView = pageView;
            this.Parameter = parameter;
            this.NavigationType = navigationType;
        }

        public NavigationType NavigationType { get; private set; }

        public object Parameter { get; private set; }

        public string PageView { get; private set; }

        public List<Task<NavigationResult>> Tasks { get; set; } = new List<Task<NavigationResult>>();
    }
}