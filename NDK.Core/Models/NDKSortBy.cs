namespace NDK.Core.Models
{
    public class NDKSortBy
    {
        private List<NDKSortModel> _data = new List<NDKSortModel>();
        public IReadOnlyCollection<NDKSortModel>? Data => _data;

        public void Add(NDKSortModel model)
        {
            _data.Add(model);
        }

        public void Remove(NDKSortModel model)
        {
            _data.Remove(model);
        }
        
        public void Clear()
        {
            _data.Clear();
        }

        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
        }

    }
}
