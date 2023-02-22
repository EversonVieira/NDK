using Microsoft.Extensions.Localization;

namespace NDK.Globalization
{
    public interface INDKStringLocalizer:IStringLocalizer
    {

        public INDKStringLocalizer SetResource(string assemblyPartialName, string resourceFile, string resourceName);
    }

}