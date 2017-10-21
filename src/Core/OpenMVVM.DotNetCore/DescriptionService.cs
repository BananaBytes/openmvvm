namespace OpenMVVM.DotNetCore
{
    using OpenMVVM.Core.PlatformServices;

    public class DescriptionService : IDescriptionService
    {
        public string Platform
        {
            get
            {
                return "DotNetCore";
            }
        }
    }
}