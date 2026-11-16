using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class ParticleSystemEmitEventStepPlayer : EventStepPlayer<ParticleSystemEmitEventStepDefinition>
    {
        private ParticleSystem target;
        private int amount;

        protected override void Init(ParticleSystemEmitEventStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
            amount = definition.Amount.Evaluate(storage);
        }

        protected override void FireEvent()
        {
            target.Emit(amount);
        }

        public override void Reset()
        {
            base.Reset();
            target = null;
            amount = default;
        }
    }
}
