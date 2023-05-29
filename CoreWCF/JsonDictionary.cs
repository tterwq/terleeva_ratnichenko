using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace WCF;

[Serializable]
public class JsonDictionary : ISerializable
{
    public Dictionary<string, object> dict;

    public JsonDictionary()
    {
        dict = new Dictionary<string, object>();
    }
    protected JsonDictionary(SerializationInfo info, StreamingContext context)
    {
        dict = new Dictionary<string, object>();
        foreach (var entry in info)
        {
            dict.Add(entry.Name, entry.Value);
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        foreach (var entry in dict)
        {
            info.AddValue(entry.Key, entry.Value);
        }
    }
}
