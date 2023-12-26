using System.Reflection;
using System.Text.RegularExpressions;

namespace SpaceBattle.Lib;

public class AdapterBuilder
{
    private string adaptername;
    private string adaptertypename;
    private string targettypename;
    private string properties = string.Empty;
    private string methods = string.Empty;

    public AdapterBuilder (Type adaptertype, Type targettype){
        this.adaptername = adaptertype.Name.Substring(1);
        this.adaptertypename = adaptertype.Name;
        this.targettypename = FormatGenericType(targettype);
    }

    public void CreateMethod(MethodInfo method){
        string methodreturnname = FormatGenericType(method.ReturnType);
        string sparameters, sparametersnames;
            
        ParameterInfo[] parameters = method.GetParameters();

        if(parameters.Length == 0){
            sparameters = string.Empty;
            sparametersnames = string.Empty;
        }
        else{
            sparameters = ", ";
            sparametersnames = ", ";
            foreach (ParameterInfo parameter in parameters){
                sparameters += @$"{FormatGenericType(parameter.ParameterType)} {parameter.Name}, ";  
                sparametersnames += @$"{parameter.Name}, ";
            }
            sparameters = sparameters.Remove(sparameters.Length-2);
            sparametersnames = sparametersnames.Remove(sparametersnames.Length-2);
        }

        if (methodreturnname == "Void"){
            methods +=
        $@"
        public void {method.Name} ({sparameters.Substring(2)}) {{
            IoC.Resolve<ICommand>(""SpaceBattle.{method.Name}.Command"", target{sparametersnames}).Execute();
        }}
            ";
        }
        else{
        methods +=
        $@"
        public {methodreturnname} {method.Name} ({sparameters}) {{
            return IoC.Resolve<{methodreturnname}>(""SpaceBattle.{method.Name}.Strategy"", target{sparametersnames});
        }}
        ";
        }
    }

    public void CreateProperty(PropertyInfo property){
        string get = string.Empty, set = string.Empty;

        string propertyname = FormatGenericType(property.PropertyType);

        if (property.CanRead){
            get = $@"   get {{ return IoC.Resolve<{propertyname}>(""SpaceBattle.{property.Name}.Get"", target); }}";
        }

        if (property.CanWrite){
            set = $@"   set {{ IoC.Resolve<ICommand>(""SpaceBattle.{property.Name}.Set"", target, value).Execute(); }}";
        }

        properties += 
        $@"
        public {propertyname} {property.Name} {{
            {get}
            {set}
        }}
        ";
    }

    private string FormatGenericType(Type type){
        string typeName = type.Name; 

        if (type.IsGenericType){
            typeName = typeName.Substring(0, typeName.IndexOf('`'));

            Type[] typeArgs = type.GetGenericArguments();
            string[] typeArgNames = new string[typeArgs.Length];
            for (int i = 0; i < typeArgs.Length; i++){
                typeArgNames[i] = FormatGenericType(typeArgs[i]);
            }
            typeName = $"{typeName}<{string.Join(", ", typeArgNames)}>";
        }
        return typeName;
    }

    public string Build(){
        string result = @$"class {adaptername}Adapter : {adaptertypename} {{
        {targettypename} target;
        public {adaptername}Adapter({targettypename} target) => this.target = target; 
        {properties}
        {methods}
    }}";

        result = Regex.Replace(result, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        return result;
    }
}
