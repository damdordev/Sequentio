using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RectTransformAnchorMaxTweenStepPlayer), "RectTransform/AnchorMax", "AnchorMax", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformAnchorMaxTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
