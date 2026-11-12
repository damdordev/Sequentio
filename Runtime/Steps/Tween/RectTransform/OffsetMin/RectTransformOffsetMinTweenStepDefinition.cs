using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening offset min on a RectTransform.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RectTransformOffsetMinTweenStepPlayer), "RectTransform/OffsetMin", "OffsetMin", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformOffsetMinTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
