using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Handlers
{
    public class NdkMiddleActionHandler
    {
        private List<Action> _beforeEventActions;
        private List<Action> _afterEventActions;

        public NdkMiddleActionHandler()
        {
            _beforeEventActions = new List<Action>();
            _afterEventActions = new List<Action>();
        }

        public void AddBeforeEventAction(Action beforeEventAction)
        {
            _beforeEventActions.Add(beforeEventAction);
        }

        public void AddAfterEventAction(Action afterEventAction)
        {
            _afterEventActions.Add(afterEventAction);
        }

        public void DropBeforeAction(Action beforeEventAction)
        {
            _beforeEventActions?.Remove(beforeEventAction);
        }

        public void DropAfterAction(Action afterEventAction)
        {
            _afterEventActions?.Remove(afterEventAction);
        }

        public void ClearBeforeEventActions()
        {
            _beforeEventActions?.Clear();
        }

        public void ClearAfterEventActions()
        {
            _afterEventActions?.Clear();
        }


        public void DoBetween(Action action)
        {
            _beforeEventActions.ForEach(a =>
            {
                a();
            });

            action();

            _afterEventActions.ForEach(a =>
            {
                a();
            });
        }


        public T DoBetweenWithResult<T>(Func<T> function)
        {
            _beforeEventActions.ForEach(a =>
            {
                a();
            });

            var result = function();

            _afterEventActions.ForEach(a =>
            {
                a();
            });

            return result;
        }
    }
}
