// https://github.com/jamesmontemagno/SettingsPlugin
namespace OpenMVVM.UWP.PlatformServices
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using global::Windows.Storage;

    using OpenMVVM.Core.PlatformServices;

    public class CacheService : ICacheService
    {
        private readonly object locker = new object();

        private static ApplicationDataContainer AppSettings => ApplicationData.Current.LocalSettings;

        /// <summary>
        /// Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        public bool AddOrUpdateValue<T>(string key, T value)
        {
            return this.InternalAddOrUpdateValue(key, value);
        }

        /// <summary>
        /// Clear all keys from the cache
        /// </summary>
        public void Clear()
        {
            lock (this.locker)
            {
                try
                {
                    AppSettings.Values.Clear();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to clear all defaults. Message: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Checks to see if the key has been added.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if contains key, else false</returns>
        public bool Contains(string key)
        {
            lock (this.locker)
            {
                try
                {
                    return AppSettings.Values.ContainsKey(key);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to check " + key + " Message: " + ex.Message);
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for the cache</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            object value;
            lock (this.locker)
            {
                if (typeof(T) == typeof(decimal))
                {
                    string savedDecimal;

                    // If the key exists, retrieve the value.
                    if (AppSettings.Values.ContainsKey(key))
                    {
                        savedDecimal = Convert.ToString(AppSettings.Values[key]);
                    }

                    // Otherwise, use the default value.
                    else
                    {
                        savedDecimal = defaultValue == null ? default(decimal).ToString() : defaultValue.ToString();
                    }

                    value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);

                    return null != value ? (T)value : defaultValue;
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    string savedTime = null;

                    // If the key exists, retrieve the value.
                    if (AppSettings.Values.ContainsKey(key))
                    {
                        savedTime = Convert.ToString(AppSettings.Values[key]);
                    }

                    if (string.IsNullOrWhiteSpace(savedTime))
                    {
                        value = defaultValue;
                    }
                    else
                    {
                        var ticks = Convert.ToInt64(savedTime, System.Globalization.CultureInfo.InvariantCulture);
                        if (ticks >= 0)
                        {
                            // Old value, stored before update to UTC values
                            value = new DateTime(ticks);
                        }
                        else
                        {
                            // New value, UTC
                            value = new DateTime(-ticks, DateTimeKind.Utc);
                        }
                    }

                    return (T)value;
                }
                else if (typeof(T) == typeof(string))
                {
                    if (!AppSettings.Values.ContainsKey(key))
                    {
                        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

                        try
                        {
                            StorageFile cachedFile = storageFolder.GetFileAsync(key).GetAwaiter().GetResult();
                            using (var inputStream = cachedFile.OpenReadAsync().GetAwaiter().GetResult())
                            using (var classicStream = inputStream.AsStreamForRead())
                            using (var streamReader = new StreamReader(classicStream))
                            {
                                return (T)Convert.ChangeType(streamReader.ReadToEnd(), typeof(T));
                            }
                        }
                        catch (Exception)
                        {                          
                        }
                    }
                }

                // If the key exists, retrieve the value.
                if (AppSettings.Values.ContainsKey(key))
                {
                    var tempValue = AppSettings.Values[key];
                    if (tempValue != null) value = (T)tempValue;
                    else value = defaultValue;
                }

                // Otherwise, use the default value.
                else
                {
                    value = defaultValue;
                }
            }

            return null != value ? (T)value : defaultValue;
        }

        /// <summary>
        /// Removes a desired key from the settings
        /// </summary>
        /// <param name="key">Key for setting</param>
        public void Remove(string key)
        {
            lock (this.locker)
            {
                // If the key exists remove
                if (AppSettings.Values.ContainsKey(key))
                {
                    AppSettings.Values.Remove(key);
                }
            }
        }

        private bool InternalAddOrUpdateValue(string key, object value)
        {
            bool valueChanged = false;
            lock (this.locker)
            {
                if (value is decimal)
                {
                    return this.AddOrUpdateValue(
                        key,
                        Convert.ToString(Convert.ToDecimal(value), System.Globalization.CultureInfo.InvariantCulture));
                }
                else if (value is DateTime)
                {
                    return this.AddOrUpdateValue(
                        key,
                        Convert.ToString(
                            -Convert.ToDateTime(value).ToUniversalTime().Ticks,
                            System.Globalization.CultureInfo.InvariantCulture));
                }
                else if (value is string && ((string)value).Length > 1024)
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile sampleFile = localFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();

                    FileIO.WriteTextAsync(sampleFile, (string)value).GetAwaiter().GetResult();
                    return true;
                }

                // If the key exists
                if (AppSettings.Values.ContainsKey(key))
                {
                    // If the value has changed
                    if (AppSettings.Values[key] != value)
                    {
                        // Store key new value
                        AppSettings.Values[key] = value;
                        valueChanged = true;
                    }
                }

                // Otherwise create the key.
                else
                {
                    AppSettings.CreateContainer(key, ApplicationDataCreateDisposition.Always);
                    AppSettings.Values[key] = value;
                    valueChanged = true;
                }
            }

            return valueChanged;
        }
    }
}