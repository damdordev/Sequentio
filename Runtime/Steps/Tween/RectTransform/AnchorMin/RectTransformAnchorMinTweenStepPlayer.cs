using UnityEngine;

namespace Damdor.Sequentio
{
    public class RectTransformAnchorMinTweenStepPlayer : Vector2TweenStepPlayer<RectTransformAnchorMinTweenStepDefinition, RectTransform>
    {
        protected override Vector2 GetValue(RectTransform component) => component.anchorMin;
        protected override void SetValue(RectTransform component, Vector2 value) => component.anchorMin = value;
    }
}
