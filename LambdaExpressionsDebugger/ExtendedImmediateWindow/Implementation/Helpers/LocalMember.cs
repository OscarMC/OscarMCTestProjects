using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uboot.ExtendedImmediateWindow.Implementation.Helpers
{
    public enum MemberType { Property, Field };

    public class LocalMember
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string TypeName { get; set; }
        public MemberType Member { get; set; }
        public bool HasGetAccessor { get; set; }
        public bool HasSetAccessor { get; set; }
        public int ImageIndex
        {
            get { return (Member == MemberType.Field ? 10 : 6); }
        }
        public string CompilableTypeName
        {
            get
            {
                return RepairTypeName(TypeName, Type);
            }
        }

        public static string RepairTypeName(string typeName, Type type)
        {
            if (type != null && type.IsGenericType)
                typeName = BuildGenericTypeName(type);

            string t = typeName.Replace("+", "."); ;
            if (t.Contains("{"))
                t = t.Substring(0, t.IndexOf("{") - 1);
            t = t.Replace("{", "").Replace("}", "").Trim();
            return t;
        }

        private static string BuildGenericTypeName(Type type)
        {
            if (type == null || type.FullName == null) return "";
            string name = type.FullName.Replace("+", ".");
            if (type.IsGenericType)
            {
                if (name.Contains("`"))
                    name = name.Substring(0, name.IndexOf("`"));
                string genArgString = "";
                var genArgs = type.GetGenericArguments();
                for (int i = 0; i < genArgs.Length; i++)
                    genArgString += (i > 0 ? ", " : "") + BuildGenericTypeName(genArgs[i]);
                name += "<" + genArgString + ">";
            }
            return name;
        }
        public string CompilableName
        {
            get
            {
                if (Name == "this")
                    return "m___this";
                return Name;
            }
        }
    }
}
