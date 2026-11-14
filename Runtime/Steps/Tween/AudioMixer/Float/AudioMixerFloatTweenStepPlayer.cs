using Damdor.Vario;
using UnityEngine;
using System;
using UnityEngine.Audio;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the AudioMixerFloatTween step.
    /// </summary>
    public class AudioMixerFloatTweenStepPlayer : FloatTweenStepPlayer<AudioMixerFloatTweenStepDefinition, AudioMixer>
    {
        private string parameterName;

        /// <inheritdoc />
        protected override void Init(AudioMixerFloatTweenStepDefinition definition, VarioStorage storage)
        {
            parameterName = definition.ParameterName.Evaluate(storage);
            base.Init(definition, storage);
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            parameterName = default;
        }

        /// <inheritdoc />
        protected override float GetValue(AudioMixer component)
        {
            if (component.GetFloat(parameterName, out var value)) return value;
            throw new InvalidOperationException($"AudioMixer parameter '{parameterName}' is not exposed or could not be read.");
        }

        /// <inheritdoc />
        protected override void SetValue(AudioMixer component, float value)
        {
            if (!component.SetFloat(parameterName, value))
                throw new InvalidOperationException($"AudioMixer parameter '{parameterName}' is not exposed or could not be written.");
        }
    }
}
