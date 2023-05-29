using System.Collections.Generic;
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

using SpaceBattle.Lib;
namespace WCF;

[DataContract(Name = "MessageContract", Namespace = "http://spacebattle.com")]
internal class MessageContract : IMessage
{
    [DataMember(Name = "type", Order = 1)]
    [OpenApiProperty(Description = "game type command")]
    public string type { get; set; }
    [DataMember(Name = "game id", Order = 2)]
    [OpenApiProperty(Description = "identificator game")]
    public string gameId { get; set; }
    [DataMember(Name = "game item id", Order = 3)]
    [OpenApiProperty(Description = "identificator game object")]
    public string gameItemId { get; set; }
    [DataMember(Name = "properties", Order = 4)]
    [OpenApiProperty(Description = "extra parameters")]
    public JsonDictionary innerDict { get; set; }
    
    public IDictionary<string, object> properties
    {
        get => (IDictionary<string, object>)innerDict.dict;
    }
}
