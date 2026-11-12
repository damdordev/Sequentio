using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening offset min on a RectTransform.
    /// </summary>
    public class RectTransformOffsetMinTweenStepPlayer : Vector2TweenStepPlayer<RectTransformOffsetMinTweenStepDefinition, RectTransform>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(RectTransform component) => component.offsetMin;
        /// <inheritdoc />
        protected override void SetValue(RectTransform component, Vector2 value) => component.offsetMin = value;
    }
}
