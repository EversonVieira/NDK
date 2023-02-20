namespace NDK.Query.Attributes
{
    public class ForeignKey: System.Attribute
    {
        public string PropertyName;

        public ForeignKey(string propertyName) 
        {
            this.PropertyName = propertyName;
        }
    }
}
