using Microsoft.AspNetCore.Components.Forms;
using NDK.UI.Components.Common;

namespace NDK.UI.Components.Inputs
{
    public partial class NDKInputDate<TValue>:NDKBaseInput<TValue>
    {

       

        public NDKInputDate()
        {
            var type = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

            if (type != typeof(DateTime) &&
                type != typeof(DateTimeOffset) &&
                type != typeof(DateOnly) &&
                type != typeof(TimeOnly))
            {
                throw new InvalidOperationException($"Unsupported {GetType()} type param '{type}'.");
            }
        }
    }
}