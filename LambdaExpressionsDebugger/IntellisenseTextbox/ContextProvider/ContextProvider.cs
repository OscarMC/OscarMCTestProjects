using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace IntellisenseTextbox
{
    public enum ContextSearchMode { Exact, CaseUnSensitive, ClosestMatch }

    public class ContextProvider
    {
        public ContextProvider(Type contextType)
        {
            Type = contextType;
            LoadContext();
            Action fireAndForgetToLowerCaching = () => Items.Select(i => i.LoweredName).ToList();
            fireAndForgetToLowerCaching.BeginInvoke(null, null);
        }

        private List<Assembly> m_assemblies = new List<Assembly>();
        private List<string> m_namespaces = new List<string>();
        private List<Type> m_types = new List<Type>();
        private List<MethodInfo> m_methods = new List<MethodInfo>();
        private List<PropertyInfo> m_properties = new List<PropertyInfo>();
        private List<FieldInfo> m_fields = new List<FieldInfo>();
        private List<EventInfo> m_events = new List<EventInfo>();
        private List<string> m_keywords = new List<string>();
        private List<string> m_knownTypes;
        private List<string> m_knownTypesIncKeywordTypes;
        private List<string> m_keywordTypes;
        private Dictionary<string, ContextItem> m_itemNameDict = new Dictionary<string, ContextItem>();
        private Dictionary<string, ContextItem> m_itemNameDictLowered = new Dictionary<string, ContextItem>();
        private Dictionary<string, ContextItem> m_itemNameDictPartialNames = new Dictionary<string, ContextItem>();

        public Type Type { get; protected set; }
        public List<ContextItem> Items { get; protected set; }
        public List<Assembly> Assemblies { get { return m_assemblies; } }
        public List<string> Keywords { get { return m_keywords; } }
        public List<string> KnownTypes { get { return m_knownTypes; } }
        public List<string> KnownTypesIncludingKeywordTypes { get { return m_knownTypesIncKeywordTypes; } }

        private void LoadContext()
        {
            //Assemblies
            m_assemblies = GetAllAssemblies(Type);
            //Namespaces
            m_namespaces = GetAllNamespaces(m_assemblies);
            //Types (= Classes, ValueTypes, Delegates, Interfaces)
            m_types = GetAllTypes(m_assemblies);
            //Methods
            m_methods = GetAllMethods(Type);
            //Properties
            m_properties = GetAllProperties(Type);
            //Fields (= Constants, Enums)
            m_fields = GetAllFields(Type);
            //Events
            m_events = GetAllEvents(Type);
            //Keywords
            m_keywords = GetAllKeywords();
            //ValueTypes
            m_keywordTypes = GetAllKeywordTypes();

            //Build Items List
            Items = GetItemsList(m_namespaces, m_types, m_methods, m_properties, m_fields, m_events, m_keywords);

            //Build known Types List
            m_knownTypes = (from i in Items where i.IsType select i.Name).ToList();
            m_knownTypesIncKeywordTypes = m_knownTypes.Union(m_keywordTypes).ToList();
            //Build namedItems Dictionaries
            m_itemNameDict = new Dictionary<string, ContextItem>();
            Items.ForEach(i => { if (!m_itemNameDict.ContainsKey(i.Name)) m_itemNameDict.Add(i.Name, i); });
            m_itemNameDictLowered = new Dictionary<string, ContextItem>();
            Items.ForEach(i => { if (!m_itemNameDict.ContainsKey(i.LoweredName)) m_itemNameDict.Add(i.LoweredName, i); });
            m_itemNameDictPartialNames = new Dictionary<string, ContextItem>();
            int maxLength = Items.Max(i => i.Name.Length);
            for (int i = 1; i < maxLength; i++)
            {
                foreach (ContextItem item in Items)
                {
                    string entry = item.LoweredName.Substring(0, Math.Min(item.LoweredName.Length - 1, i));
                    if (!m_itemNameDictPartialNames.ContainsKey(entry))
                        m_itemNameDictPartialNames.Add(entry, item);
                }
            }
        }


        private static List<ContextItem> GetItemsList(
            IEnumerable<string> namespaces,
            IEnumerable<Type> types,
            IEnumerable<MethodInfo> methods,
            IEnumerable<PropertyInfo> properties,
            IEnumerable<FieldInfo> fields,
            IEnumerable<EventInfo> events,
            IEnumerable<string> keywords)
        {
            List<ContextItem> result = new List<ContextItem>();
            try
            {
                result = (from n in namespaces
                              select new ContextItem() { SymbolType = SymbolType.Namespace, Name = n })
                    .Union(from t in types
                           where (t.IsClass || t.IsValueType) && !t.IsSubclassOf(typeof(Delegate))
                           select new ContextItem() { SymbolType = SymbolType.Class, Name = t.GetCompilerFriendlyName(), Type = t })
                    .Union(from t in types
                           where (t.IsClass || t.IsValueType) && t.IsSubclassOf(typeof(Delegate))
                           select new ContextItem() { SymbolType = SymbolType.Delegate, Name = t.GetCompilerFriendlyName(), Type = t })
                    .Union(from t in types
                           where t.IsInterface
                           select new ContextItem() { SymbolType = SymbolType.Interface, Name = t.GetCompilerFriendlyName(), Type = t })
                    .Union(from m in methods
                           select new ContextItem()
                           {
                               SymbolType = SymbolType.Method,
                               Name = m.GetCompilerFriendlyName(),
                               Type = m.ReturnType,
                               MethodOverloads = methods.Where(mi => mi.Name == m.Name).ToList()
                           })
                    .Union(from p in properties
                           select new ContextItem() { SymbolType = SymbolType.Property, Name = p.Name, Type = p.PropertyType })
                    .Union(from f in fields
                           where f.FieldType.IsSubclassOf(typeof(Enum))
                           select new ContextItem() { SymbolType = SymbolType.Enum, Name = f.Name, Type = f.FieldType })
                    .Union(from f in fields
                           where f.DeclaringType.IsSubclassOf(typeof(Enum))
                           select new ContextItem() { SymbolType = SymbolType.EnumItem, Name = f.Name, Type = f.FieldType })
                    .Union(from f in fields
                           where !f.DeclaringType.IsSubclassOf(typeof(Enum)) &&
                           !f.FieldType.IsSubclassOf(typeof(Enum)) &&
                           f.Attributes != FieldAttributes.InitOnly
                           select new ContextItem() { SymbolType = SymbolType.Field, Name = f.Name, Type = f.FieldType })
                    .Union(from f in fields
                           where !f.DeclaringType.IsSubclassOf(typeof(Enum)) &&
                           !f.FieldType.IsSubclassOf(typeof(Enum)) &&
                           f.Attributes == FieldAttributes.InitOnly
                           select new ContextItem() { SymbolType = SymbolType.Field, Name = f.Name, Type = f.FieldType })
                    .Union(from e in events
                           select new ContextItem() { SymbolType = SymbolType.Event, Name = e.Name, Type = e.EventHandlerType })
                    .Union(from k in keywords
                           select new ContextItem() { SymbolType = SymbolType.Keyword, Name = k })
                    .Where(ci => !ci.Name.Trim().StartsWith("__") && !ci.Name.StartsWith("<"))
                    .OrderBy(i => i.Name)
                    .Distinct(new ContextItem.NameComparer())
                    .ToList();
            }
            catch (Exception exc)
            {
                throw;
            }
            return result;
        }

        private List<string> GetAllKeywordTypes()
        {
            return new List<string>()
                {					
                    "var","object","delegate","string","bool","byte","float","uint","char","ulong","ushort","decimal",
                    "int","sbyte","short","double","long"
                };
        }

        public ContextItem this[string name]
        {
            get { return this[name, ContextSearchMode.ClosestMatch]; }
        }

        public ContextItem this[string name, ContextSearchMode mode]
        {
            get
            {
                if (m_itemNameDict.ContainsKey(name))
                    return m_itemNameDict[name];
                if (mode == ContextSearchMode.Exact)
                    return null;

                string loweredSearchText = name.ToLower();
                if (m_itemNameDictLowered.ContainsKey(name))
                    return m_itemNameDictLowered[name];
                if (mode == ContextSearchMode.CaseUnSensitive)
                    return null;

                for (int l = loweredSearchText.Length; l > 0; l--)
                    if (m_itemNameDictPartialNames.ContainsKey(loweredSearchText.Substring(0, l)))
                        return m_itemNameDictPartialNames[loweredSearchText.Substring(0, l)];
                return null;
            }
        }

        private static List<string> GetAllKeywords()
        {
            return new List<string>()
                {
					"abstract","event","new","struct","as","explicit","null","switch","base","extern",
                    "object","this","bool","false","operator","throw","break","finally","out","true",
                    "byte","fixed","override","try","case","float","params","typeof","catch","for",
                    "private","uint","char","foreach","protected","ulong","checked","goto","public","unchecked",
                    "class","if","readonly","unsafe","const","implicit","ref","ushort","continue","in",
                    "return","using","decimal","int","sbyte","virtual","default","interface","sealed","volatile",
                    "delegate","internal","short","void","do","is","sizeof","while","double","lock",
                    "stackalloc","else","long","static","enum","namespace","string","from","get","group",
                    "into","join","let","orderby","partial","select","set","value","var","where","yield"
                };
        }

        private static List<EventInfo> GetAllEvents(Type type)
        {
            return type.GetEvents()
                   .Union(type.GetEvents(BindingFlags.Static))
                   .Union(type.GetEvents(BindingFlags.Instance | BindingFlags.NonPublic))
                   .Union(type.GetEvents(BindingFlags.Static | BindingFlags.NonPublic))
                   .ToList();
        }

        private static List<FieldInfo> GetAllFields(Type type)
        {
            return GetAllFields(type, true, true);
        }
        private static List<FieldInfo> GetAllFields(Type type, bool staticOnes, bool instanceOnes)
        {
            return type.GetFields()
                .Union((staticOnes ? type.GetFields(BindingFlags.Static) : new FieldInfo[] { }))
                .Union((instanceOnes ? type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic) : new FieldInfo[] { }))
                .Union((staticOnes ? type.GetFields(BindingFlags.Static | BindingFlags.NonPublic) : new FieldInfo[] { }))
                .ToList();
        }
        private static List<FieldInfo> GetAllFields(Type type, bool staticOnes, bool instanceOnes, Type visibleFromType)
        {
            return GetAllFields(type, staticOnes, instanceOnes)
                .Where(f => IsVisible(f, visibleFromType))
                .ToList();                   
        }

        private static List<MethodInfo> GetAllMethods(Type type)
        {
            return GetAllMethods(type, true, true);
        }
        private static List<MethodInfo> GetAllMethods(Type type, bool staticOnes, bool instanceOnes)
        {
            return type.GetMethods()
                .Union((staticOnes ? type.GetMethods(BindingFlags.Static) : new MethodInfo[] { }))
                .Union((instanceOnes ? type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic) : new MethodInfo[] { }))
                .Union((staticOnes ? type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic) : new MethodInfo[] { }))
                .ToList();
        }
        private static List<MethodInfo> GetAllMethods(Type type, bool staticOnes, bool instanceOnes, Type visibleFromType)
        {
            return GetAllMethods(type, staticOnes, instanceOnes)
                .Where(m => 
                    IsVisible(m, visibleFromType) &&
                    !m.IsSpecialName)
                .ToList();
        }

        private static bool IsVisible(MethodInfo m, Type visibleFromType)
        {
            return m != null &&
                !m.IsConstructor &&
                (!m.IsPrivate || m.DeclaringType == visibleFromType) &&
                (!m.IsFamily || (m.DeclaringType == visibleFromType || visibleFromType.IsSubclassOf(m.DeclaringType))) &&
                (!m.IsAssembly || m.DeclaringType.Assembly == visibleFromType.Assembly);
        }
        
        private static bool IsVisible(FieldInfo fi, Type visibleFromType)
        {
            return fi != null &&
                (!fi.IsPrivate || fi.DeclaringType == visibleFromType) &&
                (!fi.IsFamily || (fi.DeclaringType == visibleFromType || visibleFromType.IsSubclassOf(fi.DeclaringType))) &&
                (!fi.IsAssembly || fi.DeclaringType.Assembly == visibleFromType.Assembly);
        }

        private static List<PropertyInfo> GetAllProperties(Type type)
        {
            return GetAllProperties(type, true, true);
        }
        private static List<PropertyInfo> GetAllProperties(Type type, bool staticOnes, bool instanceOnes)
        {
            return type.GetProperties()
                .Union((staticOnes ? type.GetProperties(BindingFlags.Static) : new PropertyInfo[] { }))
                .Union((instanceOnes ? type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic) : new PropertyInfo[] { }))
                .Union((staticOnes ? type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic) : new PropertyInfo[] { }))                
                .ToList();
        }
        private static List<PropertyInfo> GetAllProperties(Type type, bool staticOnes, bool instanceOnes, Type visibleFromType)
        {
            return GetAllProperties(type, staticOnes, instanceOnes)
                .Where(p => 
                    (p.CanRead && IsVisible(p.GetGetMethod(true), visibleFromType)) || 
                    (p.CanWrite && IsVisible(p.GetSetMethod(true), visibleFromType)))
                .ToList();                
        }

        private static List<Type> GetAllTypes(List<Assembly> assemblies)
        {
            return (from a in assemblies
                    from t in a.GetTypes()
                    select t).ToList();
        }

        private static List<string> GetAllNamespaces(IEnumerable<Assembly> assemblies)
        {
            return (from a in assemblies
                    from t in a.GetTypes()
                    where t.Namespace != null && t.Namespace.Trim().Length > 0 && !t.Namespace.StartsWith("<")
                    select t.Namespace).Distinct().ToList();
        }

        private static List<Assembly> GetAllAssemblies(Type type)
        {
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.Add(type.Assembly);
            List<string> dirs = new List<string>();
            dirs.Add(Path.GetDirectoryName(type.Assembly.Location));
            string tempDir = Path.GetTempPath();
            Func<Assembly, IList<Assembly>, IEnumerable<Assembly>> findSubElementsFunc =
                (a, list) =>
                    (from an in a.GetReferencedAssemblies()
                     select TryToLoadAssembly(tempDir, dirs, an))
                    .Where(ass => ass != null && !list.Contains(ass));
            var subAssemblies = GetRecursive<Assembly>(type.Assembly, new List<Assembly>(), findSubElementsFunc);
            assemblies.AddRange(subAssemblies);
            return assemblies;
        }

        private static Assembly TryToLoadAssembly(string tempDir, List<string> dirs, AssemblyName assemblyName)
        {
            Assembly ass = null;
            try
            {
                ass = Assembly.Load(assemblyName);
                string dir = Path.GetDirectoryName(ass.Location);
                if (!dirs.Contains(dir))
                    dirs.Add(dir);
            }
            catch
            {
                foreach (string dir in dirs)
                {
                    try
                    {
                        string file = Path.Combine(dir, assemblyName.Name + ".dll");
                        string tempPath = Path.Combine(tempDir, assemblyName.Name + ".dll");
                        File.Copy(file, tempPath);
                        ass = Assembly.LoadFile(tempPath);
                        break;
                    }
                    catch { }
                }
            }
            return ass;
        }

        private static IEnumerable<T> GetRecursive<T>(T rootElement, IList<T> alreadyFoundElements, Func<T, IList<T>, IEnumerable<T>> findSubElementsFunction)
        {
            var subElements = findSubElementsFunction(rootElement, alreadyFoundElements).ToList();
            if (subElements.Count == 0 || subElements == null) return new T[] { };
            alreadyFoundElements = alreadyFoundElements.Union(subElements).ToList();
            foreach (T subE in subElements)
                alreadyFoundElements = alreadyFoundElements.Union(GetRecursive(subE, alreadyFoundElements, findSubElementsFunction)).ToList();
            return alreadyFoundElements;
        }

        internal static IEnumerable<ContextItem> GetInstanceMembers(Type type, Type visibleFromType)
        {
            return GetItemsList(
                    new string[] { },
                    new Type[] { },
                    GetAllMethods(type, false, true, visibleFromType).Where(m => !m.IsStatic),
                    GetAllProperties(type, false, true, visibleFromType)
                        .Where(p => 
                            p.GetAccessors().Any(mi1 => IsVisible(mi1, visibleFromType)) && 
                            p.GetAccessors().Any(mi2 => !mi2.IsStatic)),
                    GetAllFields(type, false, true, visibleFromType).Where(fi => !fi.IsStatic),
                    new EventInfo[] { },
                    new string[] { });
        }

        internal static IEnumerable<ContextItem> GetStaticMembers(Type type, Type visibleFromType)
        {
            return GetItemsList(
                    new string[] { },
                    new Type[] { },
                    GetAllMethods(type, true, false, visibleFromType).Where(m => m.IsStatic),
                    GetAllProperties(type, true, false, visibleFromType)
                        .Where(p =>
                            p.GetAccessors().Any(mi1 => IsVisible(mi1, visibleFromType)) &&
                            p.GetAccessors().Any(mi2 => mi2.IsStatic)),
                    GetAllFields(type, true, false, visibleFromType).Where(fi => fi.IsStatic),
                    GetAllEvents(type),
                    new string[] { });
        }

    }
}
