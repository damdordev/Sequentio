using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening anchor min on a RectTransform.
    /// </summary>
    public class RectTransformAnchorMinTweenStepPlayer : Vector2TweenStepPlayer<RectTransformAnchorMinTweenStepDefinition, RectTransform>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(RectTransform component) => component.anchorMin;
        /// <inheritdoc />
        protected override void SetValue(RectTransform component, Vector2 value) => component.anchorMin = value;
    }
}
