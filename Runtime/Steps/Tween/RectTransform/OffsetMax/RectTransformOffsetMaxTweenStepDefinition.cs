using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RectTransformOffsetMaxTweenStepPlayer), "RectTransform/OffsetMax", "OffsetMax", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformOffsetMaxTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
