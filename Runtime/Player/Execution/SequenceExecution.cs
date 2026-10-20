using System.Collections.Generic;
using Damdor.Vario;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    public class SequenceExecution
    {
        public SequenceSettings Settings { get; set; }
        public VarioStorage Storage { get; set; }
        public List<StepExecution> Steps { get; } = new();
        public float Duration { get; set; }
        public float CurrentTime { get; set; }
        
        public void Release()
        {
            SequentioPooling.ReleaseSequenceExecution(this);
        }
        
        internal void Reset()
        {   
            foreach (var stepExecution in Steps) stepExecution.Release();
            Steps.Clear();
            
            Settings?.Release();
            Settings = null;
            
            Storage?.Release();
            Storage = null;

            CurrentTime = 0f;
        }
    }
}