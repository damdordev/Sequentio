using UnityEngine;

namespace Damdor.Sequentio
{
    public class RectTransformAnchoredPositionTweenStepPlayer : Vector2TweenStepPlayer<RectTransformAnchoredPositionTweenStepDefinition, RectTransform>
    {
        protected override Vector2 GetValue(RectTransform component) => component.anchoredPosition;
        protected override void SetValue(RectTransform component, Vector2 value) => component.anchoredPosition = value;
    }
}
