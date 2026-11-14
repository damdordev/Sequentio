using Damdor.Vario;

namespace Damdor.Sequentio
{
    public abstract class EventStepPlayer<TStepDefinition> : StepPlayer<TStepDefinition>
        where TStepDefinition : EventStepDefinition
    {
        private bool runInEveryLoop;
        private bool runInForwardDirection;
        private bool runInReverseDirection;
        private int lastLoopRun = -1;

        protected override void Init(TStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            runInEveryLoop = definition.RunInEveryLoop.Evaluate(storage);
            runInForwardDirection = definition.RunInForwardDirection.Evaluate(storage);
            runInReverseDirection = definition.RunInReverseDirection.Evaluate(storage);
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            var shouldFire = ShouldFireEvent(t);

            if (!shouldFire) return;
            lastLoopRun = ExecutionContext.LoopIndex;
            FireEvent();
        }

        public override void Reset()
        {
            base.Reset();
            lastLoopRun = -1;
        }

        protected abstract void FireEvent();
        
        private bool ShouldFireEvent(float newProgress)
        {
            if (lastLoopRun != -1 && !runInEveryLoop) return false;
            if (lastLoopRun == ExecutionContext.LoopIndex) return false;
            return (ExecutionContext.Direction == PlayDirection.Forward && runInForwardDirection && newProgress > 0.5f) ||
                   (ExecutionContext.Direction == PlayDirection.Reverse && runInReverseDirection && newProgress < 0.5f);
        }
    }
}