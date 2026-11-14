using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Light.intensity.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightIntensityTweenStepPlayer), "Light/Intensity", "Intensity", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LightIntensityTweenStepDefinition : FloatTweenStepDefinition<Light>
    {
    }
}
