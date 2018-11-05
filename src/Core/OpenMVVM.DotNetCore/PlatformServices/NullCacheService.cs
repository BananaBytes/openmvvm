using System;
using System.Collections.Generic;
using System.Text;
using OpenMVVM.Core.PlatformServices;

namespace OpenMVVM.DotNetCore.PlatformServices
{
    public class NullCacheService : ICacheService
    {
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            return default(T);
        }

        public bool AddOrUpdateValue<T>(string key, T value)
        {
            return false;
        }

        public void Remove(string key)
        {
            
        }

        public void Clear()
        {
            
        }

        public bool Contains(string key)
        {
            return false;
        }
    }
}
