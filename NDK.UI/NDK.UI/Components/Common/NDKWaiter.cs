using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Common
{
    public class NDKWaiter
    {

        private List<Func<bool>> Functions = new List<Func<bool>>();
        private List<Action> Actions = new List<Action>();

        public async Task Wait(Func<bool> waitFunction, Action act)
        {
            Functions.Clear();
            Actions.Clear();

            Functions.Add(waitFunction); 
            Actions.Add(act);

            if (Functions.Contains(waitFunction))
            {
                while (!waitFunction())
                {

                }

                if (Actions.Contains(act))
                {
                    act();
                }
            }

          

            await Task.CompletedTask;

        }
    }
}
