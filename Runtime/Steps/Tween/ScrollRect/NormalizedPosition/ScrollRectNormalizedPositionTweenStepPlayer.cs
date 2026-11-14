using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the ScrollRectNormalizedPositionTween step.
    /// </summary>
    public class ScrollRectNormalizedPositionTweenStepPlayer : Vector2TweenStepPlayer<ScrollRectNormalizedPositionTweenStepDefinition, ScrollRect>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(ScrollRect component) => component.normalizedPosition;
        /// <inheritdoc />
        protected override void SetValue(ScrollRect component, Vector2 value) => component.normalizedPosition = value;
    }
}
