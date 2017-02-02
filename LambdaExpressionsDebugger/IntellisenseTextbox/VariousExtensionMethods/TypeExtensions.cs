using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace IntellisenseTextbox
{
    public static class TypeExtensions
    {
        public static string GetCompilerFriendlyName(this Type type)
        {
            if (type == null) 
                return string.Empty;
            string name = type.Name;
            if (type.IsGenericType)
            {
                if (name.IndexOf('`') != -1)
                    name = name.Substring(0, name.IndexOf('`'));
                string typeParameters = string.Empty;
                foreach (Type t in type.GetGenericArguments())
                    typeParameters += t.GetCompilerFriendlyName() + ", ";
                if (typeParameters.Length > 0)
                    typeParameters = typeParameters.Substring(0, typeParameters.Length - 2);
                name += "<" + typeParameters + ">";                
            }
            return name;
        }

        public static string GetCompilerFriendlyName(this MethodInfo methodInfo)
        {
            return methodInfo.GetCompilerFriendlyName(false);
        }

        public static string GetCompilerFriendlyName(this MethodInfo methodInfo, bool includeParameters)
        {
            if (methodInfo == null)
                return string.Empty;
            string name = methodInfo.Name;
            if (methodInfo.IsGenericMethod)
            {
                if (name.IndexOf('`') != -1)
                    name = name.Substring(0, name.IndexOf('`'));
                string typeParameters = string.Empty;
                foreach (Type t in methodInfo.GetGenericArguments())
                    typeParameters += t.GetCompilerFriendlyName() + ", ";
                if (typeParameters.Length > 0)
                    typeParameters = typeParameters.Substring(0, typeParameters.Length - 2);
                name += "<" + typeParameters + ">";
            }
            if (includeParameters)
            {
                string paramList = "";
                foreach (var p in methodInfo.GetParameters())
                {
                    paramList += p.ToString() + ", ";
                }
                if (paramList.Length > 0)
                    paramList = paramList.Substring(0, paramList.Length - 2);
                name += "(" + paramList + ")";
            }
            return name;
        }


    }
}
