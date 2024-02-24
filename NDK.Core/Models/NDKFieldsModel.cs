namespace NDK.Core.Models
{
    public class NDKFieldsModel
    {
        private List<string> _fields = new List<string>();
        public IReadOnlyCollection<string> Fields => _fields;

        public void AddField(string name)
        {
            _fields.Add(name);
        }

        public void AddFields(IEnumerable<string> fields)
        {
            foreach (var field in fields)
            {
                _fields.Add(field);
            }
        }

        public void RemoveField(string field)
        {
            _fields.Remove(field);
        }

        public void RemoveFieldAt(int index)
        {
            _fields.RemoveAt(index);
        }

        public void ClearFields()
        {
            _fields.Clear();
        }
    }
}
