using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Light.colorTemperature.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightColorTemperatureTweenStepPlayer), "Light/ColorTemperature", "ColorTemperature", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LightColorTemperatureTweenStepDefinition : FloatTweenStepDefinition<Light>
    {
    }
}
