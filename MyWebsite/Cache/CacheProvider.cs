using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SerilogMetrics;

namespace MyWebsite.Cache
{
    /// <summary>
    /// Dummy 'cache'.
    /// </summary>
    public static class CacheProvider
    {
        private readonly static Dictionary<string, object> _internal = new Dictionary<string, object>();
        private readonly static BinaryFormatter _formatter = new BinaryFormatter();

        public static T GetItem<T>(string key)
        {
            if (_internal.ContainsKey(key))
            {
                T value = (T)_internal[key];

                LoggerFactory.GetLoggerConfiguration().CreateLogger().Information("Hit cache entry for key {key} containing value of {entrySize} bytes in length.", key, GetSize(value));

                return value;
            }

            LoggerFactory.GetLoggerConfiguration().CreateLogger().Warning("Missed cache entry for key {key}.", key);

            return default(T);
        }

        public static void PutItem<T>(string key, T value)
        {
            LoggerFactory.GetLoggerConfiguration().CreateLogger().Information("Putting item with key {key} and size {entrySize} bytes in length", key, GetSize(value));
            _internal[key] = value;
        }

        /// <summary>
        /// Gets the size of the object through a <see cref="BinaryFormatter"/>
        /// </summary>
        /// <param name="value">Value to get the size for.</param>
        /// <returns>Size of the object in byte length.</returns>
        /// <remarks></remarks>
        private static long GetSize(object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _formatter.Serialize(ms, value);
                ms.Flush();
                return ms.Length;
            }

        }
    }
}