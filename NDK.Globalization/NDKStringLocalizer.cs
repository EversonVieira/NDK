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

        private string? _resourceFile;
        private string? _assemblyPartialName;
        private string? _resourceName;

        private Assembly _assembly;
        private ResourceManager _resourceManager;

        private readonly JsonSerializer _serializer;

        public NDKStringLocalizer(string? assemblyPartialName, string? resourceFile, string? resourceName)
        {
            _assemblyPartialName = assemblyPartialName;
            _resourceFile = resourceFile;
            _resourceName = resourceName;
            _serializer = new JsonSerializer();

            if (string.IsNullOrWhiteSpace(_resourceName) || string.IsNullOrWhiteSpace(_assemblyPartialName) || string.IsNullOrWhiteSpace(_resourceFile))
            {
                throw new InvalidOperationException("AssemblyPartialName, resourceName and resourceFile are required, check and provide those data.");
            }

            _assembly = Assembly.Load(_assemblyPartialName);
            if (_assembly == null)
            {
                throw new InvalidOperationException("Assembly wasn't found");
            }

            _resourceManager = new ResourceManager(_resourceFile, _assembly);
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


        public INDKStringLocalizer SetResource(string assemblyPartialName, string resourceFile, string resourceName)
        {
            _assemblyPartialName = assemblyPartialName;
            _resourceFile = resourceFile;
            _resourceName = resourceName;

            return this;
        }

        private string? GetString(string key)
        {

            string filePath = $"{_resourceName}.{Thread.CurrentThread.CurrentCulture.Name}";
            var file = _resourceManager.GetObject(filePath) as byte[];

            if (file == null)
            {
                throw new InvalidOperationException("Resource wasn't found");

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

    public class NDKStringLocalizer<T> : NDKStringLocalizer, IStringLocalizer, INDKStringLocalizer<T>
    {
        public NDKStringLocalizer(string? assemblyPartialName, string? resourceFile, string? resourceName) : base(assemblyPartialName, resourceFile, resourceName)
        {
        }
    }
}
