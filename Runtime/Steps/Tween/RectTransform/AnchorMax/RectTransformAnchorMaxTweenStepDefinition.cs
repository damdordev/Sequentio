using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening anchor max on a RectTransform.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RectTransformAnchorMaxTweenStepPlayer), "RectTransform/AnchorMax", "AnchorMax", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformAnchorMaxTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
