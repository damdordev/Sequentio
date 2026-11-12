using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio.RunSequence
{
    /// <summary>
    /// Defines a step that controls a sequence player component.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RunSequencePlayerStepPlayer), "Sequentio/Run sequence player", "Run Sequence Player", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequencePlayerStepDefinition : StepDefinition
    {
        /// <summary>
        /// The sequence player controlled or inspected by this helper or step.
        /// </summary>
        public VarioValue<SequencePlayer> SequencePlayer
        {
            get => sequencePlayer;
            set => sequencePlayer = value;
        }
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<SequencePlayer> sequencePlayer;
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <inheritdoc />
        public override float CalculateDuration(VarioStorage storage)
        {
            var player = sequencePlayer.Evaluate(storage);
            return player!= null ? player.Duration : 0f;
        }

        /// <inheritdoc />
        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(sequencePlayer, storage);
    }
}