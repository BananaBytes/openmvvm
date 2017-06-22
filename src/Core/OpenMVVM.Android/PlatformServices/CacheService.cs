// https://github.com/jamesmontemagno/SettingsPlugin

namespace OpenMVVM.Android.PlatformServices
{
    using System;
    using System.Globalization;

    using global::Android.App;
    using global::Android.Content;
    using global::Android.Preferences;
    using global::Android.Runtime;

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
                using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context))
                {
                    using (var sharedPreferencesEditor = sharedPreferences.Edit())
                    {
                        sharedPreferencesEditor.Clear();
                        sharedPreferencesEditor.Commit();
                    }
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
                using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context))
                {
                    return sharedPreferences.Contains(key);
                }
            }
        }

        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for cache value</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            lock (this.locker)
            {
                using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context))
                {
                    return this.GetValueOrDefaultCore(sharedPreferences, key, defaultValue);
                }
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
                using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context))
                {
                    using (var sharedPreferencesEditor = sharedPreferences.Edit())
                    {
                        sharedPreferencesEditor.Remove(key);
                        sharedPreferencesEditor.Commit();
                    }
                }
            }
        }

        private bool AddOrUpdateValue(string key, object value, TypeCode typeCode)
        {
            lock (this.locker)
            {
                using (var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context))
                {
                    using (var sharedPreferencesEditor = sharedPreferences.Edit())
                    {
                        switch (typeCode)
                        {
                            case TypeCode.Decimal:
                                sharedPreferencesEditor.PutString(
                                    key,
                                    Convert.ToString(value, CultureInfo.InvariantCulture));
                                break;
                            case TypeCode.Boolean:
                                sharedPreferencesEditor.PutBoolean(key, Convert.ToBoolean(value));
                                break;
                            case TypeCode.Int64:
                                sharedPreferencesEditor.PutLong(
                                    key,
                                    (long)Convert.ToInt64(value, CultureInfo.InvariantCulture));
                                break;
                            case TypeCode.String:
                                sharedPreferencesEditor.PutString(key, Convert.ToString(value));
                                break;
                            case TypeCode.Double:
                                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                                sharedPreferencesEditor.PutString(key, valueString);
                                break;
                            case TypeCode.Int32:
                                sharedPreferencesEditor.PutInt(
                                    key,
                                    Convert.ToInt32(value, CultureInfo.InvariantCulture));
                                break;
                            case TypeCode.Single:
                                sharedPreferencesEditor.PutFloat(
                                    key,
                                    Convert.ToSingle(value, CultureInfo.InvariantCulture));
                                break;
                            case TypeCode.DateTime:
                                sharedPreferencesEditor.PutLong(
                                    key,
                                    -Convert.ToDateTime(value).ToUniversalTime().Ticks);
                                break;
                            default:
                                if (value is Guid)
                                {
                                    sharedPreferencesEditor.PutString(key, ((Guid)value).ToString());
                                }
                                else
                                {
                                    throw new ArgumentException($"Value of type {value.GetType().Name} is not supported.");
                                }

                                break;
                        }

                        sharedPreferencesEditor.Commit();
                    }
                }
            }

            return true;
        }

        private T GetValueOrDefaultCore<T>(ISharedPreferences sharedPreferences, string key, T defaultValue)
        {
            Type typeOf = typeof(T);
            if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeOf = Nullable.GetUnderlyingType(typeOf);
            }

            object value = null;
            var typeCode = Type.GetTypeCode(typeOf);
            bool resave = false;
            switch (typeCode)
            {
                case TypeCode.Decimal:

                    // Android doesn't have decimal in shared prefs so get string and convert
                    var savedDecimal = string.Empty;
                    try
                    {
                        savedDecimal = sharedPreferences.GetString(key, string.Empty);
                    }
                    catch (Java.Lang.ClassCastException)
                    {
                        Console.WriteLine("Settings 1.5 change, have to remove key.");

                        try
                        {
                            Console.WriteLine("Attempting to get old value.");
                            savedDecimal =
                                sharedPreferences.GetLong(
                                    key,
                                    (long)Convert.ToDecimal(defaultValue, CultureInfo.InvariantCulture)).ToString();
                            Console.WriteLine("Old value has been parsed and will be updated and saved.");
                        }
                        catch (Java.Lang.ClassCastException)
                        {
                            Console.WriteLine("Could not parse old value, will be lost.");
                        }

                        this.Remove(key);
                        resave = true;
                    }

                    if (string.IsNullOrWhiteSpace(savedDecimal)) value = Convert.ToDecimal(defaultValue, CultureInfo.InvariantCulture);
                    else value = Convert.ToDecimal(savedDecimal, CultureInfo.InvariantCulture);

                    if (resave) this.AddOrUpdateValue(key, value);

                    break;
                case TypeCode.Boolean:
                    value = sharedPreferences.GetBoolean(key, Convert.ToBoolean(defaultValue));
                    break;
                case TypeCode.Int64:
                    value =
                        (Int64)
                        sharedPreferences.GetLong(
                            key,
                            (long)Convert.ToInt64(defaultValue, CultureInfo.InvariantCulture));
                    break;
                case TypeCode.String:
                    value = sharedPreferences.GetString(key, Convert.ToString(defaultValue));
                    break;
                case TypeCode.Double:

                    // Android doesn't have double, so must get as string and parse.
                    var savedDouble = string.Empty;
                    try
                    {
                        savedDouble = sharedPreferences.GetString(key, string.Empty);
                    }
                    catch (Java.Lang.ClassCastException)
                    {
                        Console.WriteLine("Settings 1.5  change, have to remove key.");

                        try
                        {
                            Console.WriteLine("Attempting to get old value.");
                            savedDouble =
                                sharedPreferences.GetLong(
                                    key,
                                    (long)Convert.ToDouble(defaultValue, CultureInfo.InvariantCulture)).ToString();
                            Console.WriteLine("Old value has been parsed and will be updated and saved.");
                        }
                        catch (Java.Lang.ClassCastException)
                        {
                            Console.WriteLine("Could not parse old value, will be lost.");
                        }

                        this.Remove(key);
                        resave = true;
                    }

                    if (string.IsNullOrWhiteSpace(savedDouble)) value = defaultValue;
                    else
                    {
                        double outDouble;
                        if (
                            !double.TryParse(
                                savedDouble,
                                NumberStyles.Number,
                                CultureInfo.InvariantCulture,
                                out outDouble))
                        {
                            var maxString = Convert.ToString(double.MaxValue, CultureInfo.InvariantCulture);
                            outDouble = savedDouble.Equals(maxString) ? double.MaxValue : double.MinValue;
                        }

                        value = outDouble;
                    }

                    if (resave) this.AddOrUpdateValue(key, value);

                    break;
                case TypeCode.Int32:
                    value = sharedPreferences.GetInt(key, Convert.ToInt32(defaultValue, CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Single:
                    value = sharedPreferences.GetFloat(
                        key,
                        Convert.ToSingle(defaultValue, CultureInfo.InvariantCulture));
                    break;
                case TypeCode.DateTime:
                    if (sharedPreferences.Contains(key))
                    {
                        var ticks = sharedPreferences.GetLong(key, 0);
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
                    else
                    {
                        return defaultValue;
                    }

                    break;
                default:

                    if (defaultValue is Guid)
                    {
                        var outGuid = Guid.Empty;
                        Guid.TryParse(sharedPreferences.GetString(key, Guid.Empty.ToString()), out outGuid);
                        value = outGuid;
                    }
                    else
                    {
                        throw new ArgumentException($"Value of type {value.GetType().Name} is not supported.");
                    }

                    break;
            }

            return null != value ? (T)value : defaultValue;
        }
    }
}