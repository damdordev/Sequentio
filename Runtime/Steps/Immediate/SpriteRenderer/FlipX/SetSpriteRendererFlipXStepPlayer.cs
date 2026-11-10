using UnityEngine;

namespace Damdor.Sequentio
{
    public class SetSpriteRendererFlipXStepPlayer : ValueImmediateStepPlayer<SetSpriteRendererFlipXStepDefinition, SpriteRenderer, bool>
    {
        protected override bool GetValue(SpriteRenderer target) => target.flipX;
        protected override void SetValue(SpriteRenderer target, bool value) => target.flipX = value;
    }
}
