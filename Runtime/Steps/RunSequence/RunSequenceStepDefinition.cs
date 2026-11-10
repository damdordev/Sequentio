using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio.RunSequence
{
    [Serializable]
    [SequentioStep(typeof(RunSequenceStepPlayer), "Sequentio/Run sequence", "Run Sequence", "Packages/com.damdor.sequentio/Editor/StepsIcons/run-sequence.png")]
    public class RunSequenceStepDefinition : StepDefinition
    {
        public VarioValue<SequencePlayer> Sequence
        {
            get => sequence;
            set => sequence = value;
        }
        
        [SerializeField, SequenceStepParameterGroup(SequenceStepParameterGroups.Motion)] private VarioValue<SequencePlayer> sequence;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override float CalculateDuration(VarioStorage storage)
        {
            var definition = sequence.Evaluate(storage);
            if (!definition) return 0f;

            var plan = definition.CreateDefinitionPlan();
            var duration = plan.Duration;
            plan.Release();
            return duration;
        }

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(sequence, storage);
    }
}