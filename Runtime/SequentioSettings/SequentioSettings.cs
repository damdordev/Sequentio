using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Registers supported steps and configures step error reporting.
    /// </summary>
    public static class SequentioSettings
    {
        /// <summary>
        /// The step definition types registered with Sequentio.
        /// </summary>
        public static IReadOnlyList<Type> SupportedStepTypes => supportedStepTypes;
        /// <summary>
        /// Whether step exceptions are logged to the Unity console. Defaults to true.
        /// </summary>
        public static bool LogStepErrors { get; set; } = true;
        /// <summary>
        /// The optional callback invoked when a step operation raises an exception.
        /// </summary>
        public static SequentioStepErrorHandler StepErrorHandler { get; set; }

        /// <summary>
        /// Gets the compiler instance used to build sequence plans and executions from the registered step types.
        /// </summary>
        /// <value>
        /// The shared sequence compiler managed by the settings.
        /// </value>
        public static SequenceCompiler Compiler => compiler;

        private static readonly List<Type> supportedStepTypes = new();
        private static readonly Dictionary<Type, string> stepTypeToPath = new();
        private static readonly Dictionary<Type, string> stepTypeToName = new();
        private static readonly Dictionary<Type, string> stepToIcon = new();
        private static SequenceCompiler compiler = new();

        /// <summary>
        /// Registers a step definition, its player, and its editor metadata. Existing registrations are ignored.
        /// </summary>
        /// <typeparam name="TStepDefinition">The step definition type handled by the player or compiler.</typeparam>
        /// <typeparam name="TStepPlayer">The concrete step player type, which must have a public parameterless constructor.</typeparam>
        /// <param name="path">The hierarchical menu path used in the step picker.</param>
        /// <param name="name">The display name shown for the step.</param>
        /// <param name="icon">The optional editor asset path of the step icon.</param>
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
            Compiler.RegisterStep<TStepDefinition, TStepPlayer>();
        }

        /// <summary>
        /// Gets the registered editor display name of a step definition type.
        /// </summary>
        /// <param name="type">The step definition type whose registered metadata is requested.</param>
        /// <returns>The registered display name, or an empty string if the type is not registered.</returns>
        public static string GetStepName(Type type)
        {
            var name = stepTypeToName.GetValueOrDefault(type, "");
            return name;
        }
        
        /// <summary>
        /// Gets the registered editor menu path of a step definition type.
        /// </summary>
        /// <param name="type">The step definition type whose registered metadata is requested.</param>
        /// <returns>The registered menu path, or an empty string if the type is not registered.</returns>
        public static string GetStepPath(Type type)
        {
            var name = stepTypeToPath.GetValueOrDefault(type, "");
            return name;
        }
        
        /// <summary>
        /// Gets the registered editor icon path of a step definition type.
        /// </summary>
        /// <param name="type">The step definition type whose registered metadata is requested.</param>
        /// <returns>The registered icon path, which may be null, or an empty string if the type is not registered.</returns>
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