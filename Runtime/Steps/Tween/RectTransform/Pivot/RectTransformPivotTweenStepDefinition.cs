using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening pivot on a RectTransform.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RectTransformPivotTweenStepPlayer), "RectTransform/Pivot", "Pivot", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformPivotTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
