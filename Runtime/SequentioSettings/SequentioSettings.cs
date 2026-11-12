using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damdor.Sequentio
{
    public static class SequentioSettings
    {
        public static IReadOnlyList<Type> SupportedStepTypes => supportedStepTypes;
        public static bool LogStepErrors { get; set; }
        public static SequentioStepErrorHandler StepErrorHandler { get; set; }

        private static readonly List<Type> supportedStepTypes = new();
        private static readonly Dictionary<Type, string> stepTypeToPath = new();
        private static readonly Dictionary<Type, string> stepTypeToName = new();
        private static readonly Dictionary<Type, string> stepToIcon = new();

        public static void RegisterStep<TStepDefinition, TStepPlayer>(string path, string name, string icon)
            where TStepDefinition : StepDefinition
            where TStepPlayer : StepPlayer<TStepDefinition>, new()
        {
            var type = typeof(TStepDefinition);
            if(supportedStepTypes.Contains(type)) return;
            supportedStepTypes.Add(type);
            stepTypeToPath[type] = path;
            stepTypeToName[type] = name;
            stepToIcon[type] = icon;
            SequenceCompiler.RegisterStep<TStepDefinition, TStepPlayer>();
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

        internal static void NotifyOnStepError(SequentioStepErrorHandlerData data)
        {
            if(LogStepErrors)
            {
                Debug.LogError($"Sequentio: Step error: {data.ErrorSource}: {data.Definition.GetType().Name}: - {data.Exception}");
            }

            StepErrorHandler?.Invoke(data);
        }
        
    }
}