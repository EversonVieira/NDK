using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Features.SmartEnum
{
    public abstract class NDKSmartEnum<T>:IEquatable<NDKSmartEnum<T>>
        where T : NDKSmartEnum<T>
    {
        public int Value { get; protected init; }
        public string Name { get; protected init; } = string.Empty;

        private static readonly Dictionary<int, T> Enums = CreateEnumerations();
        protected NDKSmartEnum(int value, string name)
        {
            Value = value;
            Name = name;
        }   

        public static T? FromValue(int value)
        {
            return Enums.TryGetValue(value, out T? enumeration) ? enumeration : default;

        }

        public static T? FromName(string name)
        {
            return Enums.Values.SingleOrDefault(e => e.Name == name);
        }

        public bool Equals(NDKSmartEnum<T>? obj)
        {
            return obj is NDKSmartEnum<T> @enum &&
                   Value == @enum.Value &&
                   Name == @enum.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is NDKSmartEnum<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public int ToValue()
        {
            return this.Value;
        }

        private static Dictionary<int, T> CreateEnumerations()
        {

            var enumerationType = typeof(T);

            var fieldsForType = enumerationType
                .GetFields(BindingFlags.Public |
                           BindingFlags.Static |
                           BindingFlags.FlattenHierarchy)
                .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldInfo => (T)fieldInfo.GetValue(default)!);

            return fieldsForType.ToDictionary(x => x.Value);
        }

        public static List<NDKSmartEnum<T>?> EnumList()
        {
            return Enums.Select(x => new { Key = x.Key, Value = x.Value} as NDKSmartEnum<T>).ToList();
        }
    }
}
