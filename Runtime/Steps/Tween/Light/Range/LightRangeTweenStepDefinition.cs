using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening Light.range.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LightRangeTweenStepPlayer), "Light/Range", "Range", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LightRangeTweenStepDefinition : FloatTweenStepDefinition<Light>
    {
    }
}
