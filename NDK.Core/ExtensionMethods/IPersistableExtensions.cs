using NDK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.ExtensionMethods
{
    public static class IPersistableExtensions
    {
        public static void RefineString(this IPersistable obj)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {

                if (property.DeclaringType?.Equals(typeof(string)) ?? false)
                {
                    property.SetValue(obj, property.Name.Trim());
                }
            }
        }

        public static void RefineDate(this IPersistable obj, string startDateAlias, string endDateAlias) 
        {
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {

                if (property.DeclaringType?.Equals(typeof(DateTime)) ?? false)
                {
                    if (property.Name.Contains(startDateAlias))
                    {
                        DateTime dt = Convert.ToDateTime(property.GetValue(startDateAlias));
                        property.SetValue(obj, new DateTime(dt.Year, dt.Month, dt.Day,0,0,0, DateTimeKind.Utc));
                    }
                    else if (property.Name.Contains(endDateAlias))
                    {
                        DateTime dt = Convert.ToDateTime(property.GetValue(startDateAlias));
                        property.SetValue(obj, new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, DateTimeKind.Utc));
                    }
                }
            }
        }
    }
}
