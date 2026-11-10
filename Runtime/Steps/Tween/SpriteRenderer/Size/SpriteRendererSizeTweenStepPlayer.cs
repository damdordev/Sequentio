using UnityEngine;

namespace Damdor.Sequentio
{
    public class SpriteRendererSizeTweenStepPlayer : Vector2TweenStepPlayer<SpriteRendererSizeTweenStepDefinition, SpriteRenderer>
    {
        protected override Vector2 GetValue(SpriteRenderer component) => component.size;
        protected override void SetValue(SpriteRenderer component, Vector2 value) => component.size = value;
    }
}
