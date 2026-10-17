// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio
{
    public class StepPlan
    {
        public StepDefinition Definition { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public float Duration { get; set; }
        public bool InitOnStart { get; set; }
        public bool RunWithPreviousStep { get; set; }
        public float Delay { get; set; }
        public int OriginalIndex { get; set; }
        public int IndexInGroup { get; set; }

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