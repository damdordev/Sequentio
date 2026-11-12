using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening size delta on a RectTransform.
    /// </summary>
    public class RectTransformSizeDeltaTweenStepPlayer : Vector2TweenStepPlayer<RectTransformSizeDeltaTweenStepDefinition, RectTransform>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(RectTransform component) => component.sizeDelta;
        /// <inheritdoc />
        protected override void SetValue(RectTransform component, Vector2 value) => component.sizeDelta = value;
    }
}
