using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting flip y on a SpriteRenderer.
    /// </summary>
    public class SetSpriteRendererFlipYStepPlayer : ValueImmediateStepPlayer<SetSpriteRendererFlipYStepDefinition, SpriteRenderer, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(SpriteRenderer target) => target.flipY;
        /// <inheritdoc />
        protected override void SetValue(SpriteRenderer target, bool value) => target.flipY = value;
    }
}
