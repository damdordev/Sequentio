using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    public class EventStepDefinition : StepDefinition
    {
        public VarioValue<bool> RunInEveryLoop
        {
            get => runInEveryLoop;
            set => runInEveryLoop = value;
        }

        public VarioValue<bool> RunInForwardDirection
        {
            get => runInForwardDirection;
            set => runInForwardDirection = value;
        }

        public VarioValue<bool> RunInReverseDirection
        {
            get => runInReverseDirection;
            set => runInReverseDirection = value;
        }

        public override float CalculateDuration(VarioStorage storage) => 0f;

        [SerializeField] private VarioValue<bool> runInEveryLoop;
        [SerializeField] private VarioValue<bool> runInForwardDirection;
        [SerializeField] private VarioValue<bool> runInReverseDirection;

        public override void Reset()
        {
            base.Reset();
            runInEveryLoop = true;
            runInForwardDirection = true;
            runInReverseDirection = true;
        }
    }
}