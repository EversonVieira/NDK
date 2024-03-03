using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Common
{
    public class NDKWaiter
    {

        private Dictionary<string, Action> Actions { get; set; } = new Dictionary<string, Action>();

        public async Task Debounce(int ms, Action act, [CallerMemberName] string callerName = "")
        {
            if (Actions.ContainsKey(callerName))
            {
                Actions.Remove(callerName);
            }

            
            Actions.Add(callerName, act);
            
            await Task.Delay(ms);


            if (Actions.ContainsKey(callerName))
            {
                act();
            }
        }
    }
}
