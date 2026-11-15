using System;
using System.Collections.Generic;

namespace Damdor.Sequentio
{
    [Serializable]
    public class RunSequenceInChildrenSchedule
    {
        public IReadOnlyList<RunSequenceInChildrenSequenceSchedule> Sequences => sequences;
        public float Duration { get; private set; }

        private readonly List<RunSequenceInChildrenSequenceSchedule> sequences = new();

        internal void Reset()
        {
            sequences.Clear();
            Duration = 0f;
        }
            
        public void AddSequence(RunSequenceInChildrenSequenceSchedule sequence)
        {
            sequences.Add(sequence);
            if (sequence.EndTime > Duration) Duration = sequence.EndTime;
        }
    }
}