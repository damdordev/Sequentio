using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening anchor min on a RectTransform.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RectTransformAnchorMinTweenStepPlayer), "RectTransform/AnchorMin", "AnchorMin", "Packages/com.damdor.sequentio/Editor/StepsIcons/RectTransformAnchorMinTweenStep_icon.png")]
    public class RectTransformAnchorMinTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
