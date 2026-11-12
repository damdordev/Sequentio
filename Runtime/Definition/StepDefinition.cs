using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public abstract class StepDefinition
    {
        public VarioValue<bool> Enabled
        {
            get => enabled;
            set => enabled = value;
        }
        
        public bool InitOnStart
        {
            get => initOnStart;
            set => initOnStart = value;
        }
        
        public bool RunWithPreviousStep
        {
            get => runWithPreviousStep;
            set => runWithPreviousStep = value;
        }
        
        public VarioValue<float> Delay
        {
            get => delay;
            set => delay = value;
        }

        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private VarioValue<bool> enabled;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private VarioValue<float> delay;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private bool initOnStart;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private bool runWithPreviousStep;

        public abstract float CalculateDuration(VarioStorage storage);
        public virtual string GetTargetNameInEditor(VarioStorage storage) => "";

        public virtual void Reset()
        {
            delay = 0f;
            enabled = true;
        }
        
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