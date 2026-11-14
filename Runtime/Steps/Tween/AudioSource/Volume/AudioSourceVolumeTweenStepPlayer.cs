using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening AudioSource.volume.
    /// </summary>
    public class AudioSourceVolumeTweenStepPlayer : FloatTweenStepPlayer<AudioSourceVolumeTweenStepDefinition, AudioSource>
    {
        /// <inheritdoc />
        protected override float GetValue(AudioSource component) => component.volume;
        /// <inheritdoc />
        protected override void SetValue(AudioSource component, float value) => component.volume = value;
    }
}
