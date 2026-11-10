using UnityEngine;

namespace Damdor.Sequentio
{
    public class RectTransformSizeDeltaTweenStepPlayer : Vector2TweenStepPlayer<RectTransformSizeDeltaTweenStepDefinition, RectTransform>
    {
        protected override Vector2 GetValue(RectTransform component) => component.sizeDelta;
        protected override void SetValue(RectTransform component, Vector2 value) => component.sizeDelta = value;
    }
}
