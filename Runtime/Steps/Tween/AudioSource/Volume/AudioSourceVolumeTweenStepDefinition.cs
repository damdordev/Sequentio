using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening AudioSource.volume.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(AudioSourceVolumeTweenStepPlayer), "AudioSource/Volume", "Volume", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class AudioSourceVolumeTweenStepDefinition : FloatTweenStepDefinition<AudioSource>
    {
    }
}
