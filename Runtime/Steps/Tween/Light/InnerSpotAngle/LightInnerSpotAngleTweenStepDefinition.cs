using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Light/InnerSpotAngle step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightInnerSpotAngleTweenStepPlayer), "Light/InnerSpotAngle", "InnerSpotAngle", "Packages/com.damdor.sequentio/Editor/StepsIcons/LightInnerSpotAngleTweenStep_icon.png")]
    public class LightInnerSpotAngleTweenStepDefinition : FloatTweenStepDefinition<Light>
    {
    }
}
