using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Globalization
{
    public class NdkStringLocalizerOptions
    {
        public NdkStringLocalizerOptions(string? resourcesPath)
        {
            ResourcesPath = resourcesPath;
        }

        public string? ResourcesPath { get; set; }
    }

    public class NDKStringLocalizer : IStringLocalizer, INDKStringLocalizer
    {

        private string? _resourceName;
        private readonly IDistributedCache _cache;
        private readonly JsonSerializer _serializer;
        private readonly NdkStringLocalizerOptions _options;

        public NDKStringLocalizer(IDistributedCache cache, NdkStringLocalizerOptions options)
        {
            _cache = cache;
            _options = options;
            _serializer = new JsonSerializer();
        }

        public LocalizedString this[string name]
        {
            get
            {
                string? value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
                    : actualValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }


        public void SetResource(string resourceName, string assembly)
        {
            _resourceName = resourceName;
        }

        private string? GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(_resourceName) )
            {
                throw new InvalidOperationException("Provide a ResourceName and/or the target assembly");
            }

            string cacheKey = $"{_resourceName}_locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";

            string? cacheValue = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(cacheValue)) return cacheValue;


            string path = $@"{_options.ResourcesPath}\{_resourceName}.{Thread.CurrentThread.CurrentCulture.Name}.json";

            if (!File.Exists(path))
            {
                return default(string?);
            }
            
            using (StreamReader? reader = new StreamReader(path))
            {
                
               
                string? result = GetValueFromJSON(key, reader);

                if (!string.IsNullOrEmpty(result))
                {
                    _cache.SetString(cacheKey, result);
                }

                return result;

            }
        }

        private string? GetValueFromJSON(string propertyName, StreamReader rdr)
        {
            using (var reader = new JsonTextReader(rdr))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string?)reader.Value == propertyName)
                    {
                        reader.Read();
                        return _serializer.Deserialize<string>(reader);
                    }
                }
                return default;
            }
        }
    }
}
