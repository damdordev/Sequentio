using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening AudioSource.pitch.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(AudioSourcePitchTweenStepPlayer), "AudioSource/Pitch", "Pitch", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class AudioSourcePitchTweenStepDefinition : FloatTweenStepDefinition<AudioSource>
    {
    }
}
