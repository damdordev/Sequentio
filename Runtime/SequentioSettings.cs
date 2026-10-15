using System;
using System.Collections.Generic;

namespace Damdor.Sequentio
{
    public static class SequentioSettings
    {
        public static IReadOnlyList<Type> SupportedStepTypes => supportedStepTypes;

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

        private static readonly List<Type> supportedStepTypes = new();
        private static readonly Dictionary<Type, string> stepTypeToPath = new();
        private static readonly Dictionary<Type, string> stepTypeToName = new();
        private static readonly Dictionary<Type, string> stepToIcon = new();
        
        public static void RegisterStepType(Type type, string path, string name, string icon)
        {
            if(supportedStepTypes.Contains(type)) return;
            supportedStepTypes.Add(type);
            stepTypeToPath[type] = path;
            stepTypeToName[type] = name;
            stepToIcon[type] = icon;
        }
        
        public static string GetStepName(Type type)
        {
            var name = stepTypeToName.GetValueOrDefault(type, "");
            return name;
        }
        
        public static string GetStepPath(Type type)
        {
            var name = stepTypeToPath.GetValueOrDefault(type, "");
            return name;
        }
        
        public static string GetStepIcon(Type type)
        {
            var icon = stepToIcon.GetValueOrDefault(type, "");
            return icon;
        }
        
    }
}