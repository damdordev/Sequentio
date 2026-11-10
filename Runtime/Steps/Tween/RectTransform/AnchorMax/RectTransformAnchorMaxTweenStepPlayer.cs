using UnityEngine;

namespace Damdor.Sequentio
{
    public class RectTransformAnchorMaxTweenStepPlayer : Vector2TweenStepPlayer<RectTransformAnchorMaxTweenStepDefinition, RectTransform>
    {
        protected override Vector2 GetValue(RectTransform component) => component.anchorMax;
        protected override void SetValue(RectTransform component, Vector2 value) => component.anchorMax = value;
    }
}
