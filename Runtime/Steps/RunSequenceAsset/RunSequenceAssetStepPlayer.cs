using Damdor.Vario;

namespace Damdor.Sequentio
{
    public class RunSequenceAssetStepPlayer : StepPlayer<RunSequenceAssetStepDefinition>
    {
        private SequenceAsset sequenceAsset;
        private SequenceSettings settings;
        private RuntimeSequence runtimeSequence;
        private VarioStorage storageOverride;
        
        protected override void Init(RunSequenceAssetStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            sequenceAsset = definition.SequenceAsset.Evaluate(storage);
            settings = SequentioPooling.PopSequenceSettings();
            storageOverride = definition.StorageOverride.Clone();
            runtimeSequence = RuntimeSequence.Create(sequenceAsset, settings, ApplyStorageOverride);
            runtimeSequence.Init();
        }

        public override void SetProgress(float t)
        {
            base.SetProgress(t);
            runtimeSequence?.MoveToTime(t*runtimeSequence.Duration);
        }

        public override void ReturnToInitialState()
        {
            base.ReturnToInitialState();
            runtimeSequence?.Stop(StopBehaviour.ReturnToInitialState);
        }

        public override void Reset()
        {
            base.Reset();
            
            runtimeSequence?.Release();
            runtimeSequence = null;
            
            settings?.Release();
            settings = null;
        }
        
        private void ApplyStorageOverride(VarioStorage storage, SequenceSettings settings)
        {
            storage.CopyFrom(storageOverride);
        }
    }
}