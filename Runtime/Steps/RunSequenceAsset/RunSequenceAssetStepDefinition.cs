using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a nested sequence asset step with variable overrides.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RunSequenceAssetStepPlayer), "Sequentio/Run sequence asset", "Run Sequence Asset", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequenceAssetStepDefinition : StepDefinition
    {
        /// <summary>
        /// The sequence asset evaluated from the parent sequence storage.
        /// </summary>
        public VarioValue<SequenceAsset> SequenceAsset
        {
            get => sequenceAsset;
            set => sequenceAsset = value;
        }

        /// <summary>
        /// The variables copied over the nested or asset sequence storage when compiling its playback plan.
        /// </summary>
        public VarioStorage StorageOverride => storageOverride;
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<SequenceAsset> sequenceAsset;
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioStorage storageOverride = new();

        // ReSharper disable Unity.PerformanceAnalysis
        /// <inheritdoc />
        public override float CalculateDuration(VarioStorage storage)
        {
            var definition = sequenceAsset.Evaluate(storage);
            if (!definition) return 0f;

            var settings = SequentioPooling.PopSequenceSettings();
            var plan = SequentioSettings.Compiler.CreateDefinitionPlan(definition, settings, ApplyOverrides);
            var duration = plan.Duration;
            settings.Release();
            plan.Release();
            return duration;
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            sequenceAsset = null;
        }
        
        private void ApplyOverrides(VarioStorage storage, SequenceSettings settings)
        {
            storage.CopyFrom(storageOverride);
        }
    }
}