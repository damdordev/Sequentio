using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Light.shadowAngle.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightShadowAngleTweenStepPlayer), "Light/ShadowAngle", "ShadowAngle", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LightShadowAngleTweenStepDefinition : FloatTweenStepDefinition<Light>
    {
    }
}
