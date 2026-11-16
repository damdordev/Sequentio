using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(AudioSourceStopEventStepPlayer), "AudioSource/Stop", "Stop", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class AudioSourceStopEventStepDefinition : EventStepDefinition
    {
        public VarioValue<AudioSource> Target
        {
            get => target;
            set => target = value;
        }

        [SerializeField] private VarioValue<AudioSource> target;

        public override string GetTargetNameInEditor(VarioStorage storage) => GetTargetName(target, storage);

        public override void Reset()
        {
            base.Reset();
            target = default;
        }
    }
}
