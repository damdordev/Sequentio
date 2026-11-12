using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio.RunSequence
{
    [Serializable]
    [SequentioStep(typeof(RunSequencePlayerStepPlayer), "Sequentio/Run sequence player", "Run Sequence Player", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequencePlayerStepDefinition : StepDefinition
    {
        public VarioValue<SequencePlayer> SequencePlayer
        {
            get => sequencePlayer;
            set => sequencePlayer = value;
        }
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<SequencePlayer> sequencePlayer;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override float CalculateDuration(VarioStorage storage)
        {
            var player = sequencePlayer.Evaluate(storage);
            return player!= null ? player.Duration : 0f;
        }

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(sequencePlayer, storage);
    }
}