﻿// https://github.com/jamesmontemagno/SettingsPlugin

namespace OpenMVVM.Ios.PlatformServices
{
    using System;

    using Foundation;

    using OpenMVVM.Core.PlatformServices;

    [Preserve(AllMembers = true)]
    public class CacheService : ICacheService
    {
        private readonly object locker = new object();

        /// <summary>
        /// Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        public bool AddOrUpdateValue<T>(string key, T value)
        {
            Type typeOf = typeof(T);
            if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeOf = Nullable.GetUnderlyingType(typeOf);
            }

            var typeCode = Type.GetTypeCode(typeOf);
            return this.AddOrUpdateValue(key, value, typeCode);
        }

        /// <summary>
        /// Clear all keys from the cache
        /// </summary>
        public void Clear()
        {
            lock (this.locker)
            {
                var defaults = NSUserDefaults.StandardUserDefaults;
                try
                {
                    defaults.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
                    defaults.Synchronize();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to clear all defaults. Message: " + ex.Message);
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
                var defaults = NSUserDefaults.StandardUserDefaults;
                try
                {
                    var nsString = new NSString(key);
                    var setting = defaults.ValueForKey(nsString);
                    return setting != null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to clear all defaults. Message: " + ex.Message);
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            lock (this.locker)
            {
                var defaults = NSUserDefaults.StandardUserDefaults;

                if (defaults.ValueForKey(new NSString(key)) == null) return defaultValue;

                Type typeOf = typeof(T);
                if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeOf = Nullable.GetUnderlyingType(typeOf);
                }

                object value = null;
                var typeCode = Type.GetTypeCode(typeOf);
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        var savedDecimal = defaults.StringForKey(key);
                        value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Boolean:
                        value = defaults.BoolForKey(key);
                        break;
                    case TypeCode.Int64:
                        var savedInt64 = defaults.StringForKey(key);
                        value = Convert.ToInt64(savedInt64, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Double:
                        value = defaults.DoubleForKey(key);
                        break;
                    case TypeCode.String:
                        value = defaults.StringForKey(key);
                        break;
                    case TypeCode.Int32:
                        value = (Int32)defaults.IntForKey(key);
                        break;
                    case TypeCode.Single:
                        value = (float)defaults.FloatForKey(key);
                        break;

                    case TypeCode.DateTime:
                        var savedTime = defaults.StringForKey(key);
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

                        break;
                    default:

                        if (defaultValue is Guid)
                        {
                            var outGuid = Guid.Empty;
                            var savedGuid = defaults.StringForKey(key);
                            if (string.IsNullOrWhiteSpace(savedGuid))
                            {
                                value = outGuid;
                            }
                            else
                            {
                                Guid.TryParse(savedGuid, out outGuid);
                                value = outGuid;
                            }
                        }
                        else
                        {
                            throw new ArgumentException(
                                string.Format("Value of type {0} is not supported.", value.GetType().Name));
                        }

                        break;
                }

                return null != value ? (T)value : defaultValue;
            }
        }

        /// <summary>
        /// Removes a desired key from the cache
        /// </summary>
        /// <param name="key">Key for cache value</param>
        public void Remove(string key)
        {
            lock (this.locker)
            {
                var defaults = NSUserDefaults.StandardUserDefaults;
                try
                {
                    var nsString = new NSString(key);
                    if (defaults.ValueForKey(nsString) != null)
                    {
                        defaults.RemoveObject(key);
                        defaults.Synchronize();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
                }
            }
        }

        private bool AddOrUpdateValue(string key, object value, TypeCode typeCode)
        {
            lock (this.locker)
            {
                var defaults = NSUserDefaults.StandardUserDefaults;
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        defaults.SetString(
                            Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture),
                            key);
                        break;
                    case TypeCode.Boolean:
                        defaults.SetBool(Convert.ToBoolean(value), key);
                        break;
                    case TypeCode.Int64:
                        defaults.SetString(
                            Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture),
                            key);
                        break;
                    case TypeCode.Double:
                        defaults.SetDouble(
                            Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture),
                            key);
                        break;
                    case TypeCode.String:
                        defaults.SetString(Convert.ToString(value), key);
                        break;
                    case TypeCode.Int32:
                        defaults.SetInt(Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.Single:
                        defaults.SetFloat(
                            Convert.ToSingle(value, System.Globalization.CultureInfo.InvariantCulture),
                            key);
                        break;
                    case TypeCode.DateTime:
                        defaults.SetString(Convert.ToString(-Convert.ToDateTime(value).ToUniversalTime().Ticks), key);
                        break;
                    default:
                        if (value is Guid)
                        {
                            if (value == null) value = Guid.Empty;

                            defaults.SetString(((Guid)value).ToString(), key);
                        }
                        else
                        {
                            throw new ArgumentException($"Value of type {value.GetType().Name} is not supported.");
                        }

                        break;
                }

                try
                {
                    defaults.Synchronize();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
                }
            }

            return true;
        }
    }
}