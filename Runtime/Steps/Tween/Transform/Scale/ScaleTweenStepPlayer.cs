using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening transform local scale.
    /// </summary>
    public class ScaleTweenStepPlayer : Vector3TweenStepPlayer<ScaleTweenStepDefinition, Transform>
    {
        /// <inheritdoc />
        protected override Vector3 GetValue(Transform component) => component.localScale;
        /// <inheritdoc />
        protected override void SetValue(Transform component, Vector3 value) => component.localScale = value;
    }
}