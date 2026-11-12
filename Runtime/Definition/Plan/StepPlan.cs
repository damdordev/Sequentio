// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    /// <summary>
    /// Contains the evaluated timing and grouping information for one step.
    /// </summary>
    public class StepPlan
    {
        /// <summary>
        /// The step definition associated with this plan or execution.
        /// </summary>
        public StepDefinition Definition { get; set; }
      
        /// <summary>
        /// The time in seconds at which the step starts on the sequence timeline.
        /// </summary>
        public float StartTime { get; set; }
      
        /// <summary>
        /// The time in seconds at which the step ends on the sequence timeline.
        /// </summary>
        public float EndTime { get; set; }
        
        /// <summary>
        /// The evaluated step duration in seconds, excluding its delay.
        /// </summary>
        public float Duration { get; set; }
        
        /// <summary>
        /// Whether to initialize the step when the sequence executor is created, before its scheduled start.
        /// </summary>
        public bool InitOnStart { get; set; }
        
        /// <summary>
        /// Whether this step shares the preceding step group instead of starting a new group.
        /// </summary>
        public bool RunWithPreviousStep { get; set; }
        
        /// <summary>
        /// The delay in seconds from the start of the step group to the start of this step.
        /// </summary>
        public float Delay { get; set; }
        
        /// <summary>
        /// The zero-based index of the step in the original sequence definition.
        /// </summary>
        public int OriginalIndex { get; set; }
        
        /// <summary>
        /// The zero-based position of this step within its concurrent group.
        /// </summary>
        public int IndexInGroup { get; set; }

        /// <summary>
        /// Resets this instance and returns it to its Sequentio pool. Do not use it after release.
        /// </summary>
        public void Release()
        {
            SequentioPooling.ReleaseStepDefinitionPlan(this);
        }

        internal void Reset()
        {
            Definition = null;
            StartTime = 0f;
            EndTime = 0f;
            Duration = Delay = 0f;
            InitOnStart = false;
            RunWithPreviousStep = false;
            Delay = 0f;
            OriginalIndex = 0;
            IndexInGroup = 0;
        }
    }
}