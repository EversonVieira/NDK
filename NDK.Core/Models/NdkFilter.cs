namespace NDK.Core.Models
{
    public class NDKFilter
    {
        public int Id { get; private set; }
        private NDKRef<int> _strucutreId { get; set; }
        public NDKFilterGroup Group { get; private set; }
        public string? Target { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyName2 { get; set; }
        public object? Value { get; set; }
        public object? Value2 { get; set; }
        public NDKOperatorType OperatorType { get; set; } = NDKOperatorType.EQUAL;
        public NDKConditionType ConditionType { get; set; } = NDKConditionType.AND;

        internal NDKFilter(NDKRef<int> structureId, NDKFilterGroup filterGroup)
        {
            _strucutreId = structureId;
            Group = filterGroup;
            Id = structureId.Value;
            structureId.Value++;
        }
    }


}
