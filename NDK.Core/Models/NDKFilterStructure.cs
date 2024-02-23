namespace NDK.Core.Models
{
    public class NDKFilterStructure
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        private List<NDKFilterGroup> _groups = new List<NDKFilterGroup>();
        public IReadOnlyCollection<NDKFilterGroup> FilterGroups => _groups;

        private NDKRef<int> structureId = new NDKRef<int>()
        {
            Value = 1,
        };


        public NDKFilterGroup CreateFilterGroup()
        {
            var item = new NDKFilterGroup(structureId);


            _groups.Add(item);
            structureId.Value++;

            return item;
        }

        public void RemoveFilterGroup(NDKFilterGroup filterGroup)
        {
            _groups.Remove(filterGroup);
        }

        public void RemoveFilterGroup(int id)
        {
            var item = _groups.Find(x => x.Id == id);

            if (item != null)
            {
                _groups.Remove(item);
            }
        }

        public void ClearFilterGroups()
        {
            _groups.Clear();
        }
    }
}
