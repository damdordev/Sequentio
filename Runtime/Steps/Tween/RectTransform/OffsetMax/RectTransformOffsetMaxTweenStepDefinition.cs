using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening offset max on a RectTransform.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RectTransformOffsetMaxTweenStepPlayer), "RectTransform/OffsetMax", "OffsetMax", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformOffsetMaxTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
