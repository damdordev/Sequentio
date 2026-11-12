using System.Collections.Generic;
using Damdor.Vario;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    /// <summary>
    /// Holds the mutable playback state, settings, storage, and step players for a sequence.
    /// Mind to call <see cref="Release"/> when done with this instance.
    /// </summary>
    public class SequenceExecution
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
        public List<StepExecution> Steps { get; } = new();
        
        /// <summary>
        /// The sequence duration in seconds, excluding additional loop iterations.
        /// </summary>
        public float Duration { get; set; }
       
        /// <summary>
        /// The current position on the sequence timeline, in seconds.
        /// </summary>
        public float CurrentTime { get; set; }
        
        /// <summary>
        /// Resets this instance and returns it to its Sequentio pool. Do not use it after release.
        /// </summary>
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