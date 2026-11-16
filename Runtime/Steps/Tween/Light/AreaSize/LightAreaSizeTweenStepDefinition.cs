using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Light.areaSize.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightAreaSizeTweenStepPlayer), "Light/AreaSize", "AreaSize", "Packages/com.damdor.sequentio/Editor/StepsIcons/LightAreaSizeTweenStep_icon.png")]
    public class LightAreaSizeTweenStepDefinition : Vector2TweenStepDefinition<Light>
    {
    }
}
