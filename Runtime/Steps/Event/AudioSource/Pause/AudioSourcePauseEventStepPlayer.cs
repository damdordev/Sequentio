using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    public class AudioSourcePauseEventStepPlayer : EventStepPlayer<AudioSourcePauseEventStepDefinition>
    {
        private AudioSource target;

        protected override void Init(AudioSourcePauseEventStepDefinition definition, VarioStorage storage)
        {
            base.Init(definition, storage);
            target = definition.Target.Evaluate(storage);
        }

        protected override void FireEvent()
        {
            target.Pause();
        }

        public override void Reset()
        {
            base.Reset();
            target = null;
        }
    }
}
