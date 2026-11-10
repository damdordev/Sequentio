using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RectTransformOffsetMinTweenStepPlayer), "RectTransform/OffsetMin", "OffsetMin", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformOffsetMinTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
