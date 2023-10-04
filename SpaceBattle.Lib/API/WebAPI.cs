using CoreWCF;
using Hwdtech;

namespace SpaceBattle.Lib;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
internal class WebApi : IWebApi
{
    public void GetOperationMessage(MessageContract mc)
    {
        try
        {
            var threadId = IoC.Resolve<Dictionary<string, object>>("GetThreadID", mc);
            IoC.Resolve<ICommand>("SendCommand", threadId, IoC.Resolve<ICommand>("CreateCommandFromMessage", mc)).Execute();

        }
        catch (Exception e)
        {
            throw IoC.Resolve<Exception>("SystemException", e);
        }
    }
}
