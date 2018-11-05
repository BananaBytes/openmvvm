// https://github.com/jamesmontemagno/SettingsPlugin

namespace OpenMVVM.Core.PlatformServices
{
    using System.Threading.Tasks;

    public interface ICacheService
    {
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        T GetValueOrDefault<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for cache value</param>
        /// <param name="value">Value to set</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        bool AddOrUpdateValue<T>(string key, T value);

        /// <summary>
        /// Removes a desired key from the cache
        /// </summary>
        /// <param name="key">Key for cache value</param>
        void Remove(string key);

        /// <summary>
        /// Clear all keys from cache
        /// </summary>
        void Clear();

        /// <summary>
        /// Checks to see if the key has been added.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if contains key, else false</returns>
        bool Contains(string key);
    }
}
