using NDK.Core.Models;

namespace NDK.UI.Components.Forms
{
    public class FormData<TModel>
    {
        public TModel? Model { get; set; }
        public List<NDKMessage>? Messages { get; set; } = new List<NDKMessage>();

        public bool IsValid
        {
            get
            {
                if (Messages is null) return true;

                return !Messages.Exists(x => x.Type >= NDKMessageType.CAUTION);
            }
        }
    }
}