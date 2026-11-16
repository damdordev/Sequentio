using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening AudioSource.volume.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(AudioSourceVolumeTweenStepPlayer), "AudioSource/Volume", "Volume", "Packages/com.damdor.sequentio/Editor/StepsIcons/AudioSourceVolumeTweenStep_icon.png")]
    public class AudioSourceVolumeTweenStepDefinition : FloatTweenStepDefinition<AudioSource>
    {
    }
}
