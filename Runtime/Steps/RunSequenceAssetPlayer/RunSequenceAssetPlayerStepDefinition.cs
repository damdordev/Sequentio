using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio.RunSequence
{
    [Serializable]
    [SequentioStep(typeof(RunSequenceAssetPlayerStepPlayer), "Sequentio/Run sequence asset player", "Run Sequence Asset Player", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequenceAssetPlayerStepDefinition : StepDefinition
    {
        public VarioValue<SequenceAssetPlayer> SequencePlayer
        {
            get => sequencePlayer;
            set => sequencePlayer = value;
        }
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<SequenceAssetPlayer> sequencePlayer;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override float CalculateDuration(VarioStorage storage)
        {
            var player = sequencePlayer.Evaluate(storage);
            if (!player || player.Asset == null) return 0f;

            var plan = SequenceCompiler.CreateDefinitionPlan(player.Asset, player.Settings);
            var duration = plan.Duration;
            plan.Release();
            return duration;
        }

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(sequencePlayer, storage);
    }
}