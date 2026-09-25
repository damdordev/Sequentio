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
        
        public bool Append
        {
            get => append;
            set => append = value;
        }

        public VarioValue<float> Delay
        {
            get => delay;
            set => delay = value;
        }

        public virtual string DynamicTitle => ""; 
        
        [SerializeField] private SequentioStepVisibility visibility = new();
        [SerializeField] private bool initOnStart;
        [SerializeField] private bool append;
        [SerializeField] private VarioValue<float> delay;

        public bool IsEnabled(bool runtime) => visibility.IsEnabled(runtime);
        
        private VarioStorage storage;

        public virtual float CalculateDuration() => 0f;
        public virtual void Init() {}
        public virtual void ReturnToInitialState() {}
        public virtual void SetProgress(float t){}

        internal virtual void Reset()
        {
            Storage = null;
            visibility.Reset();
            initOnStart = false;
            append = false;
            delay = default;
        }

        internal virtual void CopyFrom(SequenceStep otherStep)
        {
            visibility.CopyFrom(otherStep.visibility);
            initOnStart = otherStep.initOnStart;
            append = otherStep.append;
            delay = otherStep.delay;
        }

        internal SequenceStep Clone()
        {
            var step = SequentioPooling.PopStep(GetType());
            step.CopyFrom(this);
            return step;
        }

        public static T Create<T>() where T : SequenceStep, new()
            => SequentioPooling.PopStep<T>();
    }
}