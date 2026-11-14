using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Light.intensity.
    /// </summary>
    public class LightIntensityTweenStepPlayer : FloatTweenStepPlayer<LightIntensityTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override float GetValue(Light component) => component.intensity;
        /// <inheritdoc />
        protected override void SetValue(Light component, float value) => component.intensity = value;
    }
}
