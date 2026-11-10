using UnityEngine;

namespace Damdor.Sequentio
{
    public class SetSpriteRendererFlipYStepPlayer : ValueImmediateStepPlayer<SetSpriteRendererFlipYStepDefinition, SpriteRenderer, bool>
    {
        protected override bool GetValue(SpriteRenderer target) => target.flipY;
        protected override void SetValue(SpriteRenderer target, bool value) => target.flipY = value;
    }
}
