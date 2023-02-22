using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Globalization
{
    public class NDKStringLocalizer : IStringLocalizer, INDKStringLocalizer
    {

        private string? _resourceName;
        private string? _assemblyPartialName;
        private string? _resourceFile;

        private readonly JsonSerializer _serializer;
        public NDKStringLocalizer()
        {
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


        public void SetResource(string assemblyPartialName, string resourceFile, string resourceName)
        {
            _assemblyPartialName = assemblyPartialName;
            _resourceFile = resourceFile;
            _resourceName = resourceName;
        }

        private string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(_resourceName))
            {
                throw new InvalidOperationException("Provide a ResourceName and/or the target assembly");
            }

            Assembly? assembly = Assembly.LoadWithPartialName(_assemblyPartialName);

            var resourceManager = new ResourceManager(_resourceFile, assembly);

            string filePath = $"{_resourceName}.{Thread.CurrentThread.CurrentCulture.Name}";
            var file = resourceManager.GetObject(filePath) as byte[];

            if (file == null)
            {
                return default(string?);
            }

            using (StreamReader? reader = new StreamReader(new MemoryStream(file)))
            {
                string? result = GetValueFromJSON(key, reader);
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
