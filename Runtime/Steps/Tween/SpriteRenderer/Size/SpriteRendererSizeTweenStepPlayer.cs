using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening size on a SpriteRenderer.
    /// </summary>
    public class SpriteRendererSizeTweenStepPlayer : Vector2TweenStepPlayer<SpriteRendererSizeTweenStepDefinition, SpriteRenderer>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(SpriteRenderer component) => component.size;
        /// <inheritdoc />
        protected override void SetValue(SpriteRenderer component, Vector2 value) => component.size = value;
    }
}
