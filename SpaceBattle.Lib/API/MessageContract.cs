using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

namespace SpaceBattle.Lib;

[DataContract(Name = "MessageContract", Namespace = "http://spacebattle.com")]
public class MessageContract
{
    [DataMember(Name = "type", Order = 1)]
    [OpenApiProperty(Description = "type of game command")]
    public string? Type { get; set; }

    [DataMember(Name = "game_ID", Order = 2)]
    [OpenApiProperty(Description = "identificator of the game")]
    public string? GameID { get; set; }

    [DataMember(Name = "game_item_ID", Order = 3)]
    [OpenApiProperty(Description = "identificator of the game object")]
    public string? GameItemID { get; set; }
    
    [DataMember(Name = "properties", Order = 4)]
    [OpenApiProperty(Description = "extra parameters")]
    public IDictionary<string, object>? Properties { get; set; }
}
