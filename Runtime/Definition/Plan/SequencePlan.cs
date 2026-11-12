// ReSharper disable Unity.PerformanceCriticalCodeInvocation

using System.Collections.Generic;
using Damdor.Vario;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Contains evaluated step timings, cloned settings, and cloned storage for a sequence.
    /// Mind to call <see cref="Release"/> when done with this instance.
    /// </summary>
    public class SequencePlan
    {
        /// <summary>
        /// The playback settings for this sequence.
        /// </summary>
        public SequenceSettings Settings { get; set; }
       
        /// <summary>
        /// The variable storage associated with this sequence.
        /// </summary>
        public VarioStorage Storage { get; set; }
        
        /// <summary>
        /// The ordered steps in this sequence.
        /// </summary>
        public List<StepPlan> Steps { get; } = new();
       
        /// <summary>
        /// The sequence duration in seconds, excluding additional loop iterations.
        /// </summary>
        public float Duration { get; set; }
        
        /// <summary>
        /// Resets this instance and returns it to its Sequentio pool. Do not use it after release.
        /// </summary>
        public void Release()
        {
            SequentioPooling.ReleaseSequenceDefinitionPlan(this);
        }
        
        internal void Reset()
        {
            Settings?.Release();
            Settings = null;
            Storage?.Release();
            Storage = null;
            
            foreach (var stepPlan in Steps) stepPlan.Release();
            Steps.Clear();
        }
    }
}