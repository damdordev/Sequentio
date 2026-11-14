using System;
using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the ScrollRect/NormalizedPosition step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(ScrollRectNormalizedPositionTweenStepPlayer), "ScrollRect/NormalizedPosition", "NormalizedPosition", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ScrollRectNormalizedPositionTweenStepDefinition : Vector2TweenStepDefinition<ScrollRect>
    {
    }
}
