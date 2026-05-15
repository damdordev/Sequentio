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

        internal void Reset()
        {
            Step = null;
        }
    }
}