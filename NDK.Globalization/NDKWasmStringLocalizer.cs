using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Globalization
{

    public class NDKWasmStringLocalizerOptions
    {
        public string ResourceFile { get; set; }

        public NDKWasmStringLocalizerOptions(string resourceFile)
        {
            ResourceFile = resourceFile;
        }
    }
    public class NDKClientStringLocalizer : IStringLocalizer, INDKStringLocalizer
    {

        private string? _resourceName;
        private readonly JsonSerializer _serializer;
        private readonly NDKWasmStringLocalizerOptions _options;
        public NDKClientStringLocalizer(NDKWasmStringLocalizerOptions options)
        {
            _serializer = new JsonSerializer();
            _options = options;
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


        public void SetResource( string resourceName)
        {
            _resourceName = resourceName;
        }


        private string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(_resourceName) )
            {
                throw new InvalidOperationException("Provide a ResourceName and/or the target assembly");
            }
            
            Assembly? assembly = Assembly.GetEntryAssembly(); 

            var resourceManager = new ResourceManager(_options.ResourceFile, assembly);

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
