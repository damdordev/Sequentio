using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RectTransformAnchoredPositionTweenStepPlayer), "RectTransform/AnchoredPosition", "AnchoredPosition", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformAnchoredPositionTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
