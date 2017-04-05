namespace OpenMVVM.Core
{
    public abstract class ViewModelLocatorBase
    {
        public static IInstanceFactory InstanceFactory => IocInstanceFactory.Default;
    }
}