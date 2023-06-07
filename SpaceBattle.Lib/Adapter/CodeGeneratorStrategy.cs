namespace SpaceBattle.Lib;
using System.Reflection;

public class AdapterCodeGeneratorStrategy : IStrategy{
    public object Strategy(params object[] args){
        Type adapterType = (Type) args[0];
        Type targetType = (Type) args[1];

        AdapterBuilder builder = new AdapterBuilder(adapterType, targetType);

        PropertyInfo[] properties = adapterType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);   

        foreach (PropertyInfo property in properties){
            builder.CreateProperty(property);
        }  

        IEnumerable<MethodInfo> methods = adapterType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(method => !method.IsSpecialName);

        foreach (MethodInfo method in methods){
            builder.CreateMethod(method);
        }

        return builder.Build();
    }
}
