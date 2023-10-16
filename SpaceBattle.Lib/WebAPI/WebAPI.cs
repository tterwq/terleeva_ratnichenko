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
            var threadID = IoC.Resolve<Dictionary<string, object>>("GetThreadID", mc);
            IoC.Resolve<ICommand>("SendCommand", threadID).Execute();

        }
        catch (Exception e)
        {
            throw IoC.Resolve<Exception>("SystemException", e);
        }
    }
}
