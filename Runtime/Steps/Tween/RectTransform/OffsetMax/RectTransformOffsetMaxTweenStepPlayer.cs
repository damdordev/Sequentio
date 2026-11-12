using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening offset max on a RectTransform.
    /// </summary>
    public class RectTransformOffsetMaxTweenStepPlayer : Vector2TweenStepPlayer<RectTransformOffsetMaxTweenStepDefinition, RectTransform>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(RectTransform component) => component.offsetMax;
        /// <inheritdoc />
        protected override void SetValue(RectTransform component, Vector2 value) => component.offsetMax = value;
    }
}
