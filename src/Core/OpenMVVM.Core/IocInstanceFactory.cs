namespace OpenMVVM.Core
{
    using System;

    using Ninject;

    public class IocInstanceFactory : IInstanceFactory
    {
        private static IocInstanceFactory instance;

        private readonly bool isWeb;

        private readonly IKernel kernel = new StandardKernel();

        protected IocInstanceFactory(bool isWeb = false)
        {
            this.isWeb = isWeb;

            this.kernel.Bind<IInstanceFactory>().ToConstant(this).InSingletonScope();
        }

        public static IInstanceFactory Default => instance ?? (instance = new IocInstanceFactory(false));

        public static IInstanceFactory DefaultWeb => instance ?? (instance = new IocInstanceFactory(true));

        public TService GetInstance<TService>()
        {
            return this.kernel.Get<TService>();
        }

        public object GetInstance(Type type)
        {
            return this.kernel.Get(type);
        }

        public TService GetInstanceByKey<TService>(string key)
            where TService : class
        {
            return this.kernel.Get<object>(name: key) as TService;
        }

        public T GetNamedInstance<T>(string key)
        {
            return this.kernel.Get<T>(key);
        }

        public void RegisterFactory<TService>(Func<TService> factory, bool isSingleton = true)
            where TService : class
        {
            // TODO: Check this implementation!
            var binding = this.kernel.Bind<TService>().ToMethod(context => factory.Invoke());

            if (isSingleton)
            {
                binding.InSingletonScope();
            }

            if (this.isWeb)
            {
                this.kernel.Get<TService>();
            }
        }

        public void RegisterInstance<TService>(TService instance, bool isSingleton = true)
            where TService : class
        {
            var binding = this.kernel.Bind<TService>().ToConstant(instance);

            if (isSingleton)
            {
                binding.InSingletonScope();
            }

            if (this.isWeb)
            {
                this.kernel.Get<TService>();
            }
        }

        public void RegisterInstanceByKey<TService>(string key)
            where TService : class
        {
            this.kernel.Bind<object>().To<TService>().Named(key);

            if (this.isWeb)
            {
                this.kernel.Get<object>(name: key);
            }
        }

        public void RegisterNamedInstance<TService>(TService instance, string key)
            where TService : class
        {
            this.kernel.Bind<TService>().ToConstant(instance).Named(key);

            if (this.isWeb)
            {
                this.kernel.Get<TService>();
            }
        }

        public void RegisterType<TInterface, TService>(bool isSingleton = true)
            where TService : class, TInterface where TInterface : class
        {
            var binding = this.kernel.Bind<TInterface>().To<TService>();

            if (isSingleton)
            {
                binding.InSingletonScope();
            }
        }

        public void RegisterType<TService>(bool isSingleton = true)
            where TService : class
        {
            var binding = this.kernel.Bind<TService>().ToSelf();

            if (isSingleton)
            {
                binding.InSingletonScope();
            }

            if (this.isWeb)
            {
                this.kernel.Get<TService>();
            }
        }
    }
}