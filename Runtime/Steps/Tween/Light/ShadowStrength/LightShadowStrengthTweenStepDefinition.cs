using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Light/ShadowStrength step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightShadowStrengthTweenStepPlayer), "Light/ShadowStrength", "ShadowStrength", "Packages/com.damdor.sequentio/Editor/StepsIcons/LightShadowStrengthTweenStep_icon.png")]
    public class LightShadowStrengthTweenStepDefinition : FloatTweenStepDefinition<Light>
    {
    }
}
