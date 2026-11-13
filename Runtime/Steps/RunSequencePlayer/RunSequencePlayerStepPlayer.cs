using Damdor.Vario;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Maps step progress to the playback time of a sequence player component.
    /// </summary>
    public class RunSequencePlayerStepPlayer : StepPlayer<RunSequencePlayerStepDefinition>
    {
        private SequencePlayer sequencePlayer;

        /// <inheritdoc />
        protected override void Init(RunSequencePlayerStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            sequencePlayer = definition.SequencePlayer.Evaluate(storage);
            sequencePlayer.Init();
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            sequencePlayer = null;
        }

        /// <inheritdoc />
        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            sequencePlayer.Stop(StopBehaviour.ReturnToInitialState);
        }
        
        /// <inheritdoc />
        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            sequencePlayer.MoveToTime(t * sequencePlayer.Duration);
        }
        
    }
}