using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening anchored position on a RectTransform.
    /// </summary>
    public class RectTransformAnchoredPositionTweenStepPlayer : Vector2TweenStepPlayer<RectTransformAnchoredPositionTweenStepDefinition, RectTransform>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(RectTransform component) => component.anchoredPosition;
        /// <inheritdoc />
        protected override void SetValue(RectTransform component, Vector2 value) => component.anchoredPosition = value;
    }
}
