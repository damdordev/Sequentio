// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    public class SequencePlan
    {
        public SequenceSettings Settings { get; set; }
        public VarioStorage Storage { get; set; }
        public List<StepPlan> Steps { get; } = new();
        public float Duration { get; set; }
        
        public void Release()
        {
            SequentioPooling.ReleaseSequenceDefinitionPlan(this);
        }
        
        internal void Reset()
        {
            Settings = null;
            Storage = null;
            
            foreach (var stepPlan in Steps) stepPlan.Release();
            Steps.Clear();
        }
    }
}