using System;
using System.Collections.Generic;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [RunSequenceInChildrenScheduler("Run with regular delay")]
    public class BaseRunWithRegularDelayScheduler : BaseRunSequenceInChildrenScheduler
    {
        public VarioValue<float> Delay
        {
            get => delay;
            set => delay = value;
        }

        public override void Schedule(VarioStorage storage, List<ISequencePlayer> sequences, RunSequenceInChildrenSchedule target)
        {
            var calculatedDelay = delay.Evaluate(storage);
            for(var i = 0; i < sequences.Count; i++)
            {
                target.AddSequence(new RunSequenceInChildrenSequenceSchedule(
                        sequences[i],
                        i * calculatedDelay,
                        i * calculatedDelay + sequences[i].Duration
                    )
                );
            }
        }

        [SerializeField] private VarioValue<float> delay;
    }
}