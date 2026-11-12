using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting flip x on a SpriteRenderer.
    /// </summary>
    public class SetSpriteRendererFlipXStepPlayer : ValueImmediateStepPlayer<SetSpriteRendererFlipXStepDefinition, SpriteRenderer, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(SpriteRenderer target) => target.flipX;
        /// <inheritdoc />
        protected override void SetValue(SpriteRenderer target, bool value) => target.flipX = value;
    }
}
