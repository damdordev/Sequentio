using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Light.bounceIntensity.
    /// </summary>
    public class LightBounceIntensityTweenStepPlayer : FloatTweenStepPlayer<LightBounceIntensityTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override float GetValue(Light component) => component.bounceIntensity;
        /// <inheritdoc />
        protected override void SetValue(Light component, float value) => component.bounceIntensity = value;
    }
}
