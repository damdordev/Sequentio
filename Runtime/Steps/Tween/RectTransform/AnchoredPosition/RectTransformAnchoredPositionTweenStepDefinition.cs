using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening anchored position on a RectTransform.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RectTransformAnchoredPositionTweenStepPlayer), "RectTransform/AnchoredPosition", "AnchoredPosition", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformAnchoredPositionTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
