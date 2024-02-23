namespace NDK.Core.Models
{
    public class NDKFilterGroup
    {
        public int Id { get; private set; }
        
        private List<NDKFilter> _filters = new List<NDKFilter>();
        public IReadOnlyCollection<NDKFilter> Filters => _filters;
        
        public NDKConditionType ConditionType { get; set; }
        
        private List<NDKFilterGroup> _InternalGroups = new List<NDKFilterGroup>();
        public IReadOnlyCollection<NDKFilterGroup> InternalGroups => _InternalGroups;
        
        
        public NDKFilterGroup? Parent { get; set; }
        private NDKRef<int> _strucutreId { get; set; }


        private List<(IdentifierType Type,int Id, object obj)> _orderList = new List<(IdentifierType Type, int Id, object obj)>();
        public IReadOnlyCollection<(IdentifierType Type, int Id, object Value)> OrderList => _orderList;

        internal NDKFilterGroup(NDKRef<int> structureId, NDKFilterGroup? filterGroup = null)
        {
            _strucutreId = new NDKRef<int> { Value = 1 };
            Id = structureId.Value;
            Parent = filterGroup;
            structureId.Value++;
        }

        public NDKFilter CreateFilter()
        {
            var item = new NDKFilter(_strucutreId, this);

            _orderList.Add(new (IdentifierType.Filter, item.Id, item));
            _filters.Add(item);


            _strucutreId.Value++;

            return item;
        }

        public NDKFilterGroup CreateFilterGroup()
        {
            var item = new NDKFilterGroup(_strucutreId,this);

            _orderList.Add(new (IdentifierType.FilterGroup, item.Id, item));
            _InternalGroups.Add(item);
            _strucutreId.Value++;

            return item;
        }

        public enum IdentifierType
        {
            Filter,
            FilterGroup,
        }
    }
}
