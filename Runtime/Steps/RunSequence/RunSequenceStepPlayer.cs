using Damdor.Vario;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Damdor.Sequentio.RunSequence
{
    public class RunSequenceStepPlayer : StepPlayer<RunSequenceStepDefinition>
    {
        private SequencePlayer sequence;
        private float duration;

        protected override void Init(RunSequenceStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            sequence = definition.Sequence.Evaluate(storage);
            sequence?.Init();
            duration = sequence?.Duration ?? 0f;
        }

        public override void Release()
        {
            base.Release();
            sequence?.Stop();
            sequence = null;
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            sequence.Stop(StopBehaviour.ReturnToInitialState);
        }
        
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            sequence?.MoveToTime(t*duration);
        }
        
    }
}