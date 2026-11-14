using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the LightSpotAngleTween step.
    /// </summary>
    public class LightSpotAngleTweenStepPlayer : FloatTweenStepPlayer<LightSpotAngleTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override float GetValue(Light component) => component.spotAngle;
        /// <inheritdoc />
        protected override void SetValue(Light component, float value) => component.spotAngle = value;
    }
}
