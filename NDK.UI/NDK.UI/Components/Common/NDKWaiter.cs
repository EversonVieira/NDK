using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Common
{
    public class NDKWaiter
    {

        private List<Action> Actions = new List<Action>();

        public async Task Debounce(int ms, Action act)
        {
            Actions.Clear();

            Actions.Add(act);
            
            await Task.Delay(ms);


            if (Actions.Contains(act))
            {
                act();
            }


            await Task.CompletedTask;

        }
    }
}
