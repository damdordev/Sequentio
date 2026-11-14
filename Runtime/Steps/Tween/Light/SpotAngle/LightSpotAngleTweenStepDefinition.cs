using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Light/SpotAngle step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightSpotAngleTweenStepPlayer), "Light/SpotAngle", "SpotAngle", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LightSpotAngleTweenStepDefinition : FloatTweenStepDefinition<Light>
    {
    }
}
