using System;
using System.Collections.Generic;

namespace Damdor.Sequentio
{
    public class SerializedStepComponent
    {
        public List<Type> Types => types;
            
        protected readonly List<Type> types = new();
        protected readonly Dictionary<Type, string> typeToName = new();

        public string GetNameForType(Type type)
        {
            return typeToName[type];
        }
    }
    
    public class SerializedStepComponent<T> : SerializedStepComponent
    {
        public void Register<T>(string name)
        {
            var type = typeof(T);
            if (types.Contains(type)) return;
            types.Add(type);
            typeToName.Add(type, name);
        }
    }
}