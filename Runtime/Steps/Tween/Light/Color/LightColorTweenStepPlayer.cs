using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Light.color.
    /// </summary>
    public class LightColorTweenStepPlayer : ColorTweenStepPlayer<LightColorTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override Color GetValue(Light component) => component.color;
        /// <inheritdoc />
        protected override void SetValue(Light component, Color value) => component.color = value;
    }
}
