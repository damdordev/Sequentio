using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Light.color.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightColorTweenStepPlayer), "Light/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LightColorTweenStepDefinition : ColorTweenStepDefinition<Light>
    {
    }
}
