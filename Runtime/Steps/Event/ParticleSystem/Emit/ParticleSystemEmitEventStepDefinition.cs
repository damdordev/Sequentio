using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(ParticleSystemEmitEventStepPlayer), "ParticleSystem/Emit", "Emit", "Packages/com.damdor.sequentio/Editor/StepsIcons/ParticleSystemEmitEventStep_icon.png")]
    public class ParticleSystemEmitEventStepDefinition : EventStepDefinition
    {
        public VarioValue<ParticleSystem> Target
        {
            get => target;
            set => target = value;
        }

        public VarioValue<int> Amount
        {
            get => amount;
            set => amount = value;
        }

        [SerializeField] private VarioValue<ParticleSystem> target;
        [SerializeField] private VarioValue<int> amount;

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(target, storage);

        public override void Reset()
        {
            base.Reset();
            target = default;
            amount = 1;
        }
    }
}
