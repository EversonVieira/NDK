using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Globalization
{
    public class NDKStringLocalizer : IStringLocalizer
    {

        private string? _resourceName;
        private string? _assembly;
        private readonly IDistributedCache _cache;
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public NDKStringLocalizer(IDistributedCache cache, string defaultResourcePath)
        {
            _cache = cache;
            _resourceName = defaultResourcePath;
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
            _assembly = assembly;
        }

        private string? GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(_resourceName) || string.IsNullOrWhiteSpace(_assembly))
            {
                throw new InvalidOperationException("Provide a ResourceName and/or the target assembly");
            }


            var assembly = Assembly.LoadFrom(_assembly);

            if (assembly is null)
            {
                throw new InvalidOperationException("Couldn't find the provided assembly");
            }

            using (Stream? stream = assembly.GetManifestResourceStream($"{_resourceName}.{Thread.CurrentThread.CurrentCulture.Name}"))
            {
                if (stream == null)
                {
                    return default(string?);
                }

                string cacheKey = $"{_assembly}_{_resourceName}_locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
                string? cacheValue = _cache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue)) return cacheValue;

                using (StreamReader reader = new StreamReader(stream))
                {
                    string? result = GetValueFromJSON(key,reader);

                    if (!string.IsNullOrEmpty(result))
                    {
                        _cache.SetString(cacheKey, result);
                    }

                    return result;
                }
               
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
