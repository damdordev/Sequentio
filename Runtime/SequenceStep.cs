using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class SequenceStep
    {
        public VarioStorage Storage
        {
            get => storage;
            set
            {
                storage = value;
                visibility.Storage = value;
            }
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
            set
            {
                delay = value;
                InvalidateDuration();
            }
        }

        public float FullDuration => fullDurationValid ? v_fullDuration : v_fullDuration = CalculateDuration();

        public virtual string TargetNameInEditor => ""; 
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private SequentioStepVisibility visibility = new();
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private bool initOnStart;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private bool runWithPreviousStep;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.General)] private VarioValue<float> delay;

        public bool IsEnabled(bool runtime) => visibility.IsEnabled(runtime);
        
        private VarioStorage storage;
        private bool fullDurationValid;
        private float v_fullDuration;
        
        protected virtual float CalculateDuration() => 0f;
        
        public virtual void Init() {}
        public virtual void SetProgress(float t){}
        public virtual void ReturnToInitialState() {}
        public virtual void Release() { }
        
        public virtual void InvalidateDuration()
        {
            fullDurationValid = false;
        }
        
        protected string GetTargetName<T>(VarioValue<T> target) where T : UnityEngine.Object
        {
            var targetName = "";
            if (typeof(T) == typeof(GameObject))
            {
                var targetGO = target.Evaluate(Storage) as GameObject;
                if (targetGO != null) targetName = targetGO.name;
                else if (target.Source == ValueSource.Storage) targetName = ":" + target.Name;
            }
            else
            {
                var targetComponent = target.Evaluate(Storage) as Component;
                if (targetComponent != null) targetName = targetComponent.name;
                else if (target.Source == ValueSource.Storage) targetName = ":" + target.Name;
            }

            return $"{targetName}";
        }
        
        internal virtual void Reset()
        {
            Storage = null;
            visibility.Reset();
            initOnStart = false;
            runWithPreviousStep = false;
            delay = default;
        }

        internal virtual void CopyFrom(SequenceStep otherStep)
        {
            visibility.CopyFrom(otherStep.visibility);
            initOnStart = otherStep.initOnStart;
            runWithPreviousStep = otherStep.runWithPreviousStep;
            delay = otherStep.delay;
        }

        internal SequenceStep Clone()
        {
            var step = SequentioPooling.PopStep(GetType());
            step.CopyFrom(this);
            return step;
        }

        private void OnValidate()
        {
            Debug.LogError("XXX");
        }
        
        public static T Create<T>() where T : SequenceStep, new()
            => SequentioPooling.PopStep<T>();
    }
}