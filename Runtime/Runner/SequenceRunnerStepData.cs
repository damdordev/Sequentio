namespace Damdor.Sequentio
{
    internal class SequenceRunnerStepData
    {
        public SequenceStep Step;
        public bool ValuesApplied;
        public float StartTime;
        public float EndTime;
        public float Duration;
        public bool Append;
        public bool InitOnStart;
        public float Delay;
        public float Progress;

        public void SetProgress(float progress)
        {
            Progress = progress;
            Step.SetProgress(progress);
        }
        
        internal void Reset()
        {
            Step = null;
            Progress = 0f;
        }
    }
}