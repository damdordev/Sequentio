using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the LightShadowStrengthTween step.
    /// </summary>
    public class LightShadowStrengthTweenStepPlayer : FloatTweenStepPlayer<LightShadowStrengthTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override float GetValue(Light component) => component.shadowStrength;
        /// <inheritdoc />
        protected override void SetValue(Light component, float value) => component.shadowStrength = value;
    }
}
