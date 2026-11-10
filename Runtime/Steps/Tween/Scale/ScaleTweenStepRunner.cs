using UnityEngine;

namespace Damdor.Sequentio
{
    public class ScaleTweenStepRunner : Vector3TweenStepPlayer<ScaleTweenStepDefinition, Transform>
    {
        protected override Vector3 GetValue(Transform component) => component.localScale;
        protected override void SetValue(Transform component, Vector3 value) => component.localScale = value;
    }
}