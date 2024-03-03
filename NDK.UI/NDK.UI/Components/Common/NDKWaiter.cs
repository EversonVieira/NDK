using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Common
{
    public static class NDKWaiter
    {
        private static Dictionary<string, Action> Actions { get; set; } = new Dictionary<string, Action>();

        public async static Task Debounce(string actionKey, int ms, Action act)
        {
            if (Actions.ContainsKey(actionKey))
            {
                Actions.Remove(actionKey);
            }

            Actions.Add(actionKey, act);
            
            await Task.Delay(ms);

            if (Actions.ContainsKey(actionKey))
            {
                act();
                Actions.Remove(actionKey);
            }
        }
    }
}
