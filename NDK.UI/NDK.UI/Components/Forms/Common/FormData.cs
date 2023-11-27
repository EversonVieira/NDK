using NDK.Core.Models;

namespace NDK.UI.Components.Forms.Common
{
    public class FormData<TModel>
    {
        public TModel? Model { get; set; }
        public List<NdkMessage>? Messages { get; set; } = new List<NdkMessage>();

        public bool IsValid
        {
            get
            {
                if (Messages is null) return true;

                return !Messages.Exists(x => x.Type >= NdkMessageType.CAUTION);
            }
        }
    }
}