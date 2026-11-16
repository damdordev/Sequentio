using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class AudioSourceUnpauseEventStepPlayer : EventStepPlayer<AudioSourceUnpauseEventStepDefinition>
    {
        private AudioSource target;

        protected override void Init(AudioSourceUnpauseEventStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
        }

        protected override void FireEvent()
        {
            target.UnPause();
        }

        public override void Reset()
        {
            base.Reset();
            target = null;
        }
    }
}
