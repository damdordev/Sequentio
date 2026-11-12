using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting sprite on a SpriteRenderer.
    /// </summary>
    public class SetSpriteRendererSpriteStepPlayer : ValueImmediateStepPlayer<SetSpriteRendererSpriteStepDefinition, SpriteRenderer, Sprite>
    {
        /// <inheritdoc />
        protected override Sprite GetValue(SpriteRenderer target) => target.sprite;
        /// <inheritdoc />
        protected override void SetValue(SpriteRenderer target, Sprite value) => target.sprite = value;
    }
}
