using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [RunSequenceInChildrenScheduler("Wait for previous sequence ended")]
    public class WaitForPreviousSequenceEnded : BaseRunSequenceInChildrenScheduler
    {
        public VarioValue<float> Delay
        {
            get => delay;
            set => delay = value;
        }
        
        public override void Schedule(VarioStorage storage, List<ISequencePlayer> sequences, RunSequenceInChildrenSchedule target)
        {
            var calculatedDelay = delay.Evaluate(storage);
            var time = 0f;
            foreach (var t in sequences)
            {
                var duration = t.Duration;
                target.AddSequence(new RunSequenceInChildrenSequenceSchedule(
                        t,
                        time,
                        time + duration
                    )
                );
                time += duration;
                time += calculatedDelay;
            }
        }
        
        [SerializeField] private VarioValue<float> delay;
    }
}