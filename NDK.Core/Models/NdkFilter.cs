namespace NDK.Core.Models
{
    public class NDKFilter
    {
        public string Id { get; set; }
        public string? Target { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyName2 { get; set; }
        public object? Value { get; set; }
        public object? Value2 { get; set; }
        public NDKOperatorType NDKOperatorType { get; set; } = NDKOperatorType.EQUAL;
        public NDKConditionType NDKConditionType { get; set; } = NDKConditionType.AND;
    }


}
