using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class AudioSourceStopEventStepPlayer : EventStepPlayer<AudioSourceStopEventStepDefinition>
    {
        private AudioSource target;

        protected override void Init(AudioSourceStopEventStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
        }

        protected override void FireEvent()
        {
            target.Stop();
        }

        public override void Reset()
        {
            base.Reset();
            target = null;
        }
    }
}
