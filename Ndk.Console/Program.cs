

using Ndk.Console;
using NDK.MiddleActions;
using System.Text.Json;


ITestMiddleActionHandler handler = new ITestMiddleActionHandler();

string query = "bla bla bla bla bla";
int x = handler.Run(() =>
{
    Console.WriteLine("Teste Middle");
    return 10;
}, () =>
{
    Console.WriteLine(query);
});

Console.ReadKey();


public class ITestMiddleActionHandler : MiddleActionHandler<ITest>
{

    public ITestMiddleActionHandler()
    {
        Action Before = () => Console.WriteLine("Início");

        Action After = () => Console.WriteLine("Final");


        Test test = new Test();
        BridgeAction bridgeAction = new BridgeAction(() =>
        {
            test.Counter++;
            Console.WriteLine("Bridge Start");
            Console.WriteLine(JsonSerializer.Serialize(test));
        }, () => {

            test.Counter++;
            Console.WriteLine("Bridge End");
            Console.WriteLine(JsonSerializer.Serialize(test));

        });



        base.AddBeforeAction(Before);
        base.AddAfterAction(After);
        base.AddBridgeAction(bridgeAction);



    }

}





