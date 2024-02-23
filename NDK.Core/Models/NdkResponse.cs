using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class NDKResponse
    {
        public List<NDKMessage> Messages { get; set; } = new List<NDKMessage>();

        public bool HasAnyMessage => Messages.Count > 0;

        public bool HasMessageByType(NDKMessageType type)
        {
            return Messages.Exists(x => x.Type == type);
        }

        public bool HasStopFlowMessages => Messages.Exists(x => x.Type > NDKMessageType.CAUTION);


        public bool HasMessageByTypes(params NDKMessageType[] types)
        {
            bool result = false;

            foreach (NDKMessageType type in types)
            {
                result = result || HasMessageByType(type);
            }

            return result;
        }

        public void AddMessage(NDKMessage message)
        {
            Messages.Add(message);
        }

      
    }

    public class NDKResponse<T> : NDKResponse
    {
        public T? Result { get; set; }

    }
}
