namespace NDK.Globalization
{
    public class NDKStringLocalizerOptions
    {
        public string ResourceFile { get; set; }
        public NDKStringLocalizerOptions(string resourceFile)
        {
            ResourceFile = resourceFile;
        }
    }
}
