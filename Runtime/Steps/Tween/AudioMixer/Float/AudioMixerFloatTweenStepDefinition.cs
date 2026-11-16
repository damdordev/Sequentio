using System;
using Damdor.Vario;
using UnityEngine;
using UnityEngine.Audio;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the AudioMixer/Float step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(AudioMixerFloatTweenStepPlayer), "AudioMixer/Float", "AudioMixer Float", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class AudioMixerFloatTweenStepDefinition : FloatTweenStepDefinition<AudioMixer>
    {
        /// <summary>
        /// The exposed AudioMixer parameter to animate.
        /// </summary>
        public VarioValue<string> ParameterName
        {
            get => parameterName;
            set => parameterName = value;
        }

        [SerializeField] private VarioValue<string> parameterName;

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            parameterName = default;
        }
    }
}
