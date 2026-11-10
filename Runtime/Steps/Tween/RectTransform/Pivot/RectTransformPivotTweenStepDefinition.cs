using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RectTransformPivotTweenStepPlayer), "RectTransform/Pivot", "Pivot", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformPivotTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
