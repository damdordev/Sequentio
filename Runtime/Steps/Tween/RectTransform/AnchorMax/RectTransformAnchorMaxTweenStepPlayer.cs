using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening anchor max on a RectTransform.
    /// </summary>
    public class RectTransformAnchorMaxTweenStepPlayer : Vector2TweenStepPlayer<RectTransformAnchorMaxTweenStepDefinition, RectTransform>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(RectTransform component) => component.anchorMax;
        /// <inheritdoc />
        protected override void SetValue(RectTransform component, Vector2 value) => component.anchorMax = value;
    }
}
