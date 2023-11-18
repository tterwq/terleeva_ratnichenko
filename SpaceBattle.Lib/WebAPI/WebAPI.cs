using CoreWCF;
using Hwdtech;

namespace SpaceBattle.Lib;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class WebApi : IWebApi
{
    public void GetOperationMessage(MessageContract mc)
    {
        try
        {
            var obj= IoC.Resolve<Dictionary<string, object>>("SerializeContract", mc);
            IoC.Resolve<ICommand>("SendCommand", obj).Execute();

        }
        catch (Exception e)
        {
            throw IoC.Resolve<Exception>("SystemException", e);
        }
    }
}
