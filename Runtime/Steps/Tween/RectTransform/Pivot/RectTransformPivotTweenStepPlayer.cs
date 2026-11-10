using UnityEngine;

namespace Damdor.Sequentio
{
    public class RectTransformPivotTweenStepPlayer : Vector2TweenStepPlayer<RectTransformPivotTweenStepDefinition, RectTransform>
    {
        protected override Vector2 GetValue(RectTransform component) => component.pivot;
        protected override void SetValue(RectTransform component, Vector2 value) => component.pivot = value;
    }
}
