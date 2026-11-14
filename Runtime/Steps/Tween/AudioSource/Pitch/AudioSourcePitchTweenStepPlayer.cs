using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening AudioSource.pitch.
    /// </summary>
    public class AudioSourcePitchTweenStepPlayer : FloatTweenStepPlayer<AudioSourcePitchTweenStepDefinition, AudioSource>
    {
        /// <inheritdoc />
        protected override float GetValue(AudioSource component) => component.pitch;
        /// <inheritdoc />
        protected override void SetValue(AudioSource component, float value) => component.pitch = value;
    }
}
