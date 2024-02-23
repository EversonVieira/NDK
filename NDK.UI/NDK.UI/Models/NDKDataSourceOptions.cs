namespace NDK.UI.Models
{
    public class NDKDataSourceOptions<TOutput, TSignature>
    {
        public bool DataBasePager { get; set; }
        public bool DataBaseSort { get; set; }
        public bool DataBaseFilter { get; set; }

        /// <summary>
        /// Provide a function to remove the Selected Data from the current List
        /// Parameter: Input to be checked (by the engine).
        /// Return: True if should keep, False if should remove
        /// </summary>
        public Func<TOutput, bool>? RemoveSelectedDataFunction { get; set; }
    }

    public class NDKDataSourceOptions<TOutput>:NDKDataSourceOptions<TOutput,object>
    {

    }
}
