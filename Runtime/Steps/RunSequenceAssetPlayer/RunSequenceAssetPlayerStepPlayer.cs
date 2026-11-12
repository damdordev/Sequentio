using Damdor.Vario;

namespace Damdor.Sequentio.RunSequence
{
    /// <summary>
    /// Maps step progress to the playback time of a sequence asset player component.
    /// </summary>
    public class RunSequenceAssetPlayerStepPlayer : StepPlayer<RunSequenceAssetPlayerStepDefinition>
    {
        private SequenceAssetPlayer sequenceAssetPlayer;

        /// <inheritdoc />
        protected override void Init(RunSequenceAssetPlayerStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            sequenceAssetPlayer = definition.SequencePlayer.Evaluate(storage);
            sequenceAssetPlayer.Init();
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            sequenceAssetPlayer = null;
        }

        /// <inheritdoc />
        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            sequenceAssetPlayer.Stop(StopBehaviour.ReturnToInitialState);
        }
        
        /// <inheritdoc />
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            sequenceAssetPlayer.MoveToTime(t * sequenceAssetPlayer.Duration);
        }
        
    }
}