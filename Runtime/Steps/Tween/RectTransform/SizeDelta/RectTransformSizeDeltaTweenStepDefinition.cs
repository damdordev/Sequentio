using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(RectTransformSizeDeltaTweenStepPlayer), "RectTransform/SizeDelta", "SizeDelta", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformSizeDeltaTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
