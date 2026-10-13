using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep("Delay", "Delay")]
    public class DelayStep : SequenceStep
    {
        public override float CalculateDuration() => duration.Evaluate(Storage);

        [SerializeField] private VarioValue<float> duration;

        internal override void CopyFrom(SequenceStep otherStep)
        {
            base.CopyFrom(otherStep);
            if (otherStep is not DelayStep other) return;

            duration = other.duration;
        }

        public override void Init()
        {
            base.Init();
            duration = 0f;
        }
    }
}