using CoreWCF;
using Hwdtech;

namespace WCF;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
internal class WebApi : IWebApi
{
    public void GetOperationMessage(MessageContract param)
    {
        try
        {
            var threadId = IoC.Resolve<string>("GetThreadID", param.gameId);
            IoC.Resolve<SpaceBattle.Lib.ICommand>("SendCommand", threadId, IoC.Resolve<SpaceBattle.Lib.ICommand>("CreateMessage", param)).Execute();
        }
        catch (System.Exception e)
        {
            throw IoC.Resolve<System.Exception>("SystemException", e);
        }
    }
}
