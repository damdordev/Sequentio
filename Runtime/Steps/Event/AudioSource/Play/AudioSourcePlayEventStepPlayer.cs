using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class AudioSourcePlayEventStepPlayer : EventStepPlayer<AudioSourcePlayEventStepDefinition>
    {
        private AudioSource target;

        protected override void Init(AudioSourcePlayEventStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
        }

        protected override void FireEvent()
        {
            target.Play();
        }

        public override void Reset()
        {
            base.Reset();
            target = null;
        }
    }
}
