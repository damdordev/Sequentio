using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step that controls a sequence asset player component.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RunSequenceAssetPlayerStepPlayer), "Sequentio/Run sequence asset player", "Run Sequence Asset Player", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequenceAssetPlayerStepDefinition : StepDefinition
    {
        /// <summary>
        /// The sequence player controlled or inspected by this helper or step.
        /// </summary>
        public VarioValue<SequenceAssetPlayer> SequencePlayer
        {
            get => sequencePlayer;
            set => sequencePlayer = value;
        }
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<SequenceAssetPlayer> sequencePlayer;
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <inheritdoc />
        public override float CalculateDuration(VarioStorage storage)
        {
            var player = sequencePlayer.Evaluate(storage);
            if (!player) return 0f;
            return !player ? 0f : player.Duration;
        }

        /// <inheritdoc />
        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(sequencePlayer, storage);
    }
}