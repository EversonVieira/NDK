using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.MiddleActions
{
    
    public abstract class MiddleActionHandler<Signature> 
    {
        private List<Action> beforeActions = new List<Action>();
        private List<Action> afterActions = new List<Action>();
        private List<BridgeAction> bridgeActions = new List<BridgeAction>();

        protected void AddAfterAction(Action action)
        {
            afterActions.Add(action);
        }

        protected void AddBeforeAction(Action action)
        {
            beforeActions.Add(action);
        }

        protected void AddBridgeAction(BridgeAction bridgeAction)
        {
            bridgeActions.Add(bridgeAction);
        }

        protected void RemoveAfterAction(Action action)
        {
            afterActions.Remove(action);
        }

        protected void RemoveBeforeAction(Action action)
        {
            beforeActions.Remove(action);
        }

        protected void RemoveBridgeActions(BridgeAction bridgeAction)
        {
            bridgeActions.Remove(bridgeAction);
        }

        public void Run(Action action, Action? OnFinishedStatement = null)
        {
            beforeActions.ForEach(e => {e();});

            bridgeActions.ForEach(a => { a.BeforeAction(); });

            action();

            bridgeActions.ForEach(b => { b.AfterAction(); });

            afterActions.ForEach(e => { e(); });

            if (OnFinishedStatement != null)
            {
                OnFinishedStatement();
            }

        }

        public T Run<T>(Func<T> func, Action? OnFinishedStatement = null)
        {
            beforeActions.ForEach(e => { e(); });

            bridgeActions.ForEach(a => { a.BeforeAction(); });

            T result = func();

            bridgeActions.ForEach(b => { b.AfterAction(); });

            afterActions.ForEach(e => { e(); });

            if (OnFinishedStatement != null)
            {
                OnFinishedStatement();
            }
            return result;
        }
    }
}
