using System.Net;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace SpaceBattle.Lib;

[ServiceContract]
[OpenApiBasePath("/api")]
public interface IWebApi
{
    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/orders")]
    [OpenApiTag("Tag")]
    [OpenApiResponse(ContentTypes = new[] { "application/json", "text/xml" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(MessageContract))]
   
    void GetOperationMessage([OpenApiParameter(ContentTypes = new[] { "application/json", "text/xml" }, Description = "mc description.")] MessageContract mc);
}
