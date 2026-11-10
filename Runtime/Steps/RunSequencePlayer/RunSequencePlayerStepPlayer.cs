using Damdor.Vario;

namespace Damdor.Sequentio.RunSequence
{
    public class RunSequencePlayerStepPlayer : StepPlayer<RunSequencePlayerStepDefinition>
    {
        private SequencePlayer sequencePlayer;

        protected override void Init(RunSequencePlayerStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            sequencePlayer = definition.SequencePlayer.Evaluate(storage);
            sequencePlayer.Init();
        }

        public override void Reset()
        {
            base.Reset();
            sequencePlayer = null;
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            sequencePlayer.Stop(StopBehaviour.ReturnToInitialState);
        }
        
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            sequencePlayer.MoveToTime(t * sequencePlayer.Duration);
        }
        
    }
}