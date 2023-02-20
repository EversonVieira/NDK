namespace NDK.Core.Models
{
    public class NdkFilter
    {
        public string Id { get; set; }
        public string? Target { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyName2 { get; set; }
        public object? Value { get; set; }
        public object? Value2 { get; set; }
        public NdkOperatorType NdkOperatorType { get; set; } = NdkOperatorType.EQUAL;
        public NdkConditionType NdkConditionType { get; set; } = NdkConditionType.AND;
    }


}
