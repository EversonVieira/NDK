namespace NDK.MiddleActions
{
    public class BridgeAction
    {
        public Action BeforeAction { get; private set; }
        public Action AfterAction { get; private set; }

        public BridgeAction(Action beforeAction, Action afterAction)
        {
            BeforeAction = beforeAction;
            AfterAction = afterAction;
        }
    }
}
