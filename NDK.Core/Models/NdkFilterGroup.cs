namespace NDK.Core.Models
{
    public class NdkFilterGroup
    {
        public string? Id { get; set; }
        public List<NdkFilter>? Filters { get; set; }
        public NdkConditionType ConditionType { get; set; }
        public List<NdkFilterGroup>? InternalGroups { get; set; }
    }


}
