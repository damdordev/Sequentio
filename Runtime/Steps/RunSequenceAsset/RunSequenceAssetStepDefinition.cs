using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RunSequenceAssetStepPlayer), "Sequentio/Run sequence asset", "Run Sequence Asset", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequenceAssetStepDefinition : StepDefinition
    {
        public VarioValue<SequenceAsset> SequenceAsset
        {
            get => sequenceAsset;
            set => sequenceAsset = value;
        }

        public VarioStorage StorageOverride => storageOverride;
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<SequenceAsset> sequenceAsset;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioStorage storageOverride = new();

        // ReSharper disable Unity.PerformanceAnalysis
        public override float CalculateDuration(VarioStorage storage)
        {
            var definition = sequenceAsset.Evaluate(storage);
            if (!definition) return 0f;

            var settings = SequentioPooling.PopSequenceSettings();
            var plan = definition.CreateDefinitionPlan(settings);
            var duration = plan.Duration;
            settings.Release();
            plan.Release();
            return duration;
        }
        
        public override void Reset()
        {
            base.Reset();
            sequenceAsset = null;
            
            storageOverride.Release();
            storageOverride = null;
        }
    }
}