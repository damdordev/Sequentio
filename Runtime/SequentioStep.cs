using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class SequentioStep
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
        
        [SerializeField] private SequentioStepVisibility visibility;
        [SerializeField] private bool initOnStart;
        [SerializeField] private bool append;
        [SerializeField] private VarioValue<float> delay;

        public bool IsEnabled(bool runtime) => visibility.IsEnabled(runtime);
        
        private VarioStorage storage;

        public virtual float CalculateDuration() => 0f;
        public virtual void ApplyValues() {}
        public virtual void SaveInitialState() {}
        public virtual void ReturnToInitialState() {}
        public virtual void SetProgress(float t){}
        
    }
}