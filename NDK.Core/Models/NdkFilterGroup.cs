namespace NDK.Core.Models
{
    public class NDKFilterGroup
    {
        public string? Id { get; set; }
        public List<NDKFilter>? Filters { get; set; }
        public NDKConditionType ConditionType { get; set; }
        public List<NDKFilterGroup>? InternalGroups { get; set; }
    }


}
