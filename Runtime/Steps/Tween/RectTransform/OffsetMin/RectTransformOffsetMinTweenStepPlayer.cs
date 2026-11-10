using UnityEngine;

namespace Damdor.Sequentio
{
    public class RectTransformOffsetMinTweenStepPlayer : Vector2TweenStepPlayer<RectTransformOffsetMinTweenStepDefinition, RectTransform>
    {
        protected override Vector2 GetValue(RectTransform component) => component.offsetMin;
        protected override void SetValue(RectTransform component, Vector2 value) => component.offsetMin = value;
    }
}
