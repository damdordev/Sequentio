using UnityEngine;

namespace Damdor.Sequentio
{
    public class SetSpriteRendererSpriteStepPlayer : ValueImmediateStepPlayer<SetSpriteRendererSpriteStepDefinition, SpriteRenderer, Sprite>
    {
        protected override Sprite GetValue(SpriteRenderer target) => target.sprite;
        protected override void SetValue(SpriteRenderer target, Sprite value) => target.sprite = value;
    }
}
