using Microsoft.Extensions.Localization;

namespace NDK.Globalization
{
    public interface INDKStringLocalizer:IStringLocalizer
    {

        public void SetResource(string assemblyPartialName, string resourceFile, string resourceName);
    }

}