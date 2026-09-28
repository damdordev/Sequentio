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
    }
}