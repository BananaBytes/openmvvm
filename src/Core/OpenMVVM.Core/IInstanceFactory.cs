namespace OpenMVVM.Core
{
    using System;

    public interface IInstanceFactory
    {
        void RegisterType<TInterface, TService>(bool isSingleton = true)
            where TService : class, TInterface
            where TInterface : class;

        void RegisterType<TService>(bool isSingleton = true)
            where TService : class;

        void RegisterInstance<TService>(TService instance, bool isSingleton = true)
            where TService : class;

        void RegisterNamedInstance<TService>(TService instance, string key)
            where TService : class;

        TService GetInstance<TService>();

        TService GetNamedInstance<TService>(string key);

        void RegisterFactory<TService>(Func<TService> factory, bool isSingleton = true)
            where TService : class;

        object GetInstance(Type type);

        void RegisterInstanceByKey<TService>(string key)
            where TService : class;

        TService GetInstanceByKey<TService>(string key)
            where TService : class;
    }
}
