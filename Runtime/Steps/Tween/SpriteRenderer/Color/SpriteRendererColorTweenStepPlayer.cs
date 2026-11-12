using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening color on a SpriteRenderer.
    /// </summary>
    public class SpriteRendererColorTweenStepPlayer : ColorTweenStepPlayer<SpriteRendererColorTweenStepDefinition, SpriteRenderer>
    {
        /// <inheritdoc />
        protected override Color GetValue(SpriteRenderer component) => component.color;
        /// <inheritdoc />
        protected override void SetValue(SpriteRenderer component, Color value) => component.color = value;
    }
}
