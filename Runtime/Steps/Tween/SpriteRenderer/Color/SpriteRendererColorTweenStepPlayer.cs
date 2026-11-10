using UnityEngine;

namespace Damdor.Sequentio
{
    public class SpriteRendererColorTweenStepPlayer : ColorTweenStepPlayer<SpriteRendererColorTweenStepDefinition, SpriteRenderer>
    {
        protected override Color GetValue(SpriteRenderer component) => component.color;
        protected override void SetValue(SpriteRenderer component, Color value) => component.color = value;
    }
}
