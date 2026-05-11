using System;
using System.Collections.Generic;
using System.Reflection;

namespace Damdor.Sequentio
{
    public static class SequentioSettings
    {
        public static IReadOnlyList<Type> SupportedStepTypes
        {
            get
            {
                EnsureSupportedTypesInit();
                return supportedStepTypes;
            }
        }

        public static int StepsPoolSize
        {
            get => SequentioPooling.StepsPoolSize;
            set => SequentioPooling.StepsPoolSize = value;
        }
        
        public static int RawSequencePoolSize
        {
            get => SequentioPooling.RawSequencePoolSize;
            set => SequentioPooling.RawSequencePoolSize = value;
        }
        
        public static int SettingsPoolSize
        {
            get => SequentioPooling.SettingsPoolSize;
            set => SequentioPooling.SettingsPoolSize = value;
        }

        private static List<Type> supportedStepTypes;
        private static readonly Dictionary<Type, string> stepTypeToName = new();
       
        public static string GetStepName(Type type)
        {
            EnsureSupportedTypesInit();
            var name = stepTypeToName.GetValueOrDefault(type, "");
            return name;
        }
        
        private static void EnsureSupportedTypesInit()
        {
            if (supportedStepTypes != null) return;
            
            var baseType = typeof(SequenceStep);

            supportedStepTypes = new();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if(type.IsInterface || type.IsAbstract || !type.IsSerializable || !baseType.IsAssignableFrom(type)) continue;
                    var attr = type.GetCustomAttribute<SequentioStepAttribute>();
                    if(attr == null) continue;
                    
                    RegisterStepType(type, attr.Name);
                }
            }

        }
        
        private static void RegisterStepType(Type type, string name)
        {
            if(supportedStepTypes.Contains(type)) return;
            supportedStepTypes.Add(type);
            stepTypeToName[type] = name;
        }
        
    }
}