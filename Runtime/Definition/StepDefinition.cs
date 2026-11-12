using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the timing, initialization, and behavior of a sequence step.
    /// </summary>
    [Serializable]
    public abstract class StepDefinition
    {
        /// <summary>
        /// The condition evaluated during compilation to determine whether this step is included.
        /// </summary>
        public VarioValue<bool> Enabled
        {
            get => enabled;
            set => enabled = value;
        }
        
        /// <summary>
        /// Whether to initialize the step when the sequence executor is created, before its scheduled start.
        /// </summary>
        public bool InitOnStart
        {
            get => initOnStart;
            set => initOnStart = value;
        }
        
        /// <summary>
        /// Whether this step shares the preceding step group instead of starting a new group.
        /// </summary>
        public bool RunWithPreviousStep
        {
            get => runWithPreviousStep;
            set => runWithPreviousStep = value;
        }
        
        /// <summary>
        /// The delay in seconds from the start of the step group to the start of this step.
        /// </summary>
        public VarioValue<float> Delay
        {
            get => delay;
            set => delay = value;
        }

        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private VarioValue<bool> enabled;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private VarioValue<float> delay;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private bool initOnStart;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private bool runWithPreviousStep;

        /// <summary>
        /// Evaluates the duration of this step using the supplied variable storage.
        /// </summary>
        /// <param name="storage">The variable storage used to resolve step parameters.</param>
        /// <returns>The step duration in seconds, excluding its delay.</returns>
        public abstract float CalculateDuration(VarioStorage storage);
        
        /// <summary>
        /// Gets a target label for display in the step inspector.
        /// </summary>
        /// <param name="storage">The variable storage used to resolve step parameters.</param>
        /// <returns>The target label, or an empty string when no label is supplied.</returns>
        public virtual string GetTargetNameInEditor(VarioStorage storage) => "";

        /// <summary>
        /// Resets the definition to its default parameter values.
        /// </summary>
        public virtual void Reset()
        {
            delay = 0f;
            enabled = true;
        }
        
        /// <summary>
        /// Resolves a Unity target name, falling back to a storage variable name when the target is unavailable.
        /// </summary>
        /// <typeparam name="T">The target type whose editor name is resolved.</typeparam>
        /// <param name="target">The target whose value is read or written.</param>
        /// <param name="storage">The variable storage used to resolve step parameters.</param>
        /// <returns>The target name or a colon-prefixed storage variable name; otherwise an empty string.</returns>
        protected string GetTargetName<T>(VarioValue<T> target, VarioStorage storage)
        {
            var targetName = "";
            if (typeof(T) == typeof(GameObject))
            {
                var targetGO = target.Evaluate(storage) as GameObject;
                if (targetGO) targetName = targetGO.name;
                else if (target.Source == ValueSource.Storage) targetName = ":" + target.Name;
            }
            else
            {
                var targetComponent = target.Evaluate(storage) as Component;
                if (targetComponent) targetName = targetComponent.name;
                else if (target.Source == ValueSource.Storage) targetName = ":" + target.Name;
            }

            return $"{targetName}";
        }
        
    }
}