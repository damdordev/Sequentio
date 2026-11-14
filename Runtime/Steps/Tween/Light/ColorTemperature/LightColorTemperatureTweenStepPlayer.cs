using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Light.colorTemperature.
    /// </summary>
    public class LightColorTemperatureTweenStepPlayer : FloatTweenStepPlayer<LightColorTemperatureTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override float GetValue(Light component) => component.colorTemperature;
        /// <inheritdoc />
        protected override void SetValue(Light component, float value) => component.colorTemperature = value;
    }
}
