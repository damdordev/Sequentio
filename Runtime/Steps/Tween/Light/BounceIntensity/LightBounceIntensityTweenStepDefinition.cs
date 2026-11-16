using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Light.bounceIntensity.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightBounceIntensityTweenStepPlayer), "Light/BounceIntensity", "BounceIntensity", "Packages/com.damdor.sequentio/Editor/StepsIcons/LightBounceIntensityTweenStep_icon.png")]
    public class LightBounceIntensityTweenStepDefinition : FloatTweenStepDefinition<Light>
    {
    }
}
