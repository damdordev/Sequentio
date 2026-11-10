using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio.RunSequence
{
    [Serializable]
    [SequentioStep(typeof(RunSequencePlayerStepPlayer), "Sequentio/Run sequence player", "Run Sequence player", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
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
            if (!player) return 0f;

            var plan = player.CreateDefinitionPlan();
            var duration = plan.Duration;
            plan.Release();
            return duration;
        }

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(sequencePlayer, storage);
    }
}