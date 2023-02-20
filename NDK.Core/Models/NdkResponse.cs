using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class NdkResponse
    {
        public List<NdkMessage> Messages { get; set; } = new List<NdkMessage>();

        public bool HasAnyMessage => Messages.Any();

        public bool HasMessageByType(NdkMessageType type)
        {
            return Messages.Any(x => x.Type == type);
        }


        public bool HasMessageByTypes(params NdkMessageType[] types)
        {
            bool result = false;

            foreach (NdkMessageType type in types)
            {
                result = result || HasMessageByType(type);
            }

            return result;
        }

        public void AddMessage(NdkMessage message)
        {
            Messages.Add(message);
        }
    }

    public class NdkResponse<T> : NdkResponse
    {
        public T? Result { get; set; }

    }
}
