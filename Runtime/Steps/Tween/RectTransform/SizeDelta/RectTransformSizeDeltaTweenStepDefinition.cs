using System;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening size delta on a RectTransform.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(RectTransformSizeDeltaTweenStepPlayer), "RectTransform/SizeDelta", "SizeDelta", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class RectTransformSizeDeltaTweenStepDefinition : Vector2TweenStepDefinition<RectTransform>
    {
    }
}
