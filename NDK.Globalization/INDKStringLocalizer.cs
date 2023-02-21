using Microsoft.Extensions.Localization;

namespace NDK.Globalization
{
    public interface INDKStringLocalizer:IStringLocalizer
    {
      
        void SetResource(string resourceName, string assembly);
    }
}