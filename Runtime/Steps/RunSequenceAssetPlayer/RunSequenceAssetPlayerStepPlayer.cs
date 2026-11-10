using Damdor.Vario;

namespace Damdor.Sequentio.RunSequence
{
    public class RunSequenceAssetPlayerStepPlayer : StepPlayer<RunSequenceAssetPlayerStepDefinition>
    {
        private SequenceAssetPlayer sequenceAssetPlayer;

        protected override void Init(RunSequenceAssetPlayerStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            sequenceAssetPlayer = definition.SequencePlayer.Evaluate(storage);
            sequenceAssetPlayer.Init();
        }

        public override void Reset()
        {
            base.Reset();
            sequenceAssetPlayer = null;
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            sequenceAssetPlayer.Stop(StopBehaviour.ReturnToInitialState);
        }
        
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            sequenceAssetPlayer.MoveToTime(t * sequenceAssetPlayer.Duration);
        }
        
    }
}