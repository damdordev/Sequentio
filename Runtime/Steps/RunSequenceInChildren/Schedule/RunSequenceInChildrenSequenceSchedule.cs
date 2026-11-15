namespace Damdor.Sequentio
{
    public readonly struct RunSequenceInChildrenSequenceSchedule
    {
        public readonly ISequencePlayer Sequence { get; }
        public readonly float StartTime { get; }
        public readonly float EndTime { get; }
            
        public RunSequenceInChildrenSequenceSchedule(ISequencePlayer sequence, float startTime, float endTime)
        {
            Sequence = sequence;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}