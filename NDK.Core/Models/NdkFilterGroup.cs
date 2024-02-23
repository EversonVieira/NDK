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


        private List<InternalMap> _orderList = new List<InternalMap>();
        public IReadOnlyCollection<InternalMap> OrderList => _orderList;

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

            _orderList.FindAll(x => x.IsLastOne && x.Type == IdentifierType.Filter).ForEach(e =>
            {
                e.IsLastOne = false;
            });
            _orderList.Add(new InternalMap
            {
                Type = IdentifierType.Filter,
                Id = item.Id,
                Value = item,
                IsLastOne = true
            });


            _filters.Add(item);


            _strucutreId.Value++;

            return item;
        }

        public NDKFilterGroup CreateFilterGroup()
        {
            var item = new NDKFilterGroup(_strucutreId, this);


            _orderList.FindAll(x => x.IsLastOne && x.Type == IdentifierType.FilterGroup).ForEach(e =>
            {
                e.IsLastOne = false;
            });

            _orderList.Add(new InternalMap
            {
                Type = IdentifierType.FilterGroup,
                Id = item.Id,
                Value = item,
                IsLastOne = true
            });

            _InternalGroups.Add(item);
            _strucutreId.Value++;

            return item;
        }

        public enum IdentifierType
        {
            Filter,
            FilterGroup,
        }

        public class InternalMap
        {
            public IdentifierType Type { get; set; }
            public int Id { get; set; }
            public object? Value { get; set; }
            public bool IsLastOne { get; set; }
        }
    }
}
