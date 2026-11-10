using UnityEngine;

namespace Damdor.Sequentio
{
    public class RectTransformOffsetMaxTweenStepPlayer : Vector2TweenStepPlayer<RectTransformOffsetMaxTweenStepDefinition, RectTransform>
    {
        protected override Vector2 GetValue(RectTransform component) => component.offsetMax;
        protected override void SetValue(RectTransform component, Vector2 value) => component.offsetMax = value;
    }
}
