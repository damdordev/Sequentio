using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening pivot on a RectTransform.
    /// </summary>
    public class RectTransformPivotTweenStepPlayer : Vector2TweenStepPlayer<RectTransformPivotTweenStepDefinition, RectTransform>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(RectTransform component) => component.pivot;
        /// <inheritdoc />
        protected override void SetValue(RectTransform component, Vector2 value) => component.pivot = value;
    }
}
