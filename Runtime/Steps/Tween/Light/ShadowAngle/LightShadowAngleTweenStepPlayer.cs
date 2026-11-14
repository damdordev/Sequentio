using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Light.shadowAngle.
    /// </summary>
    public class LightShadowAngleTweenStepPlayer : FloatTweenStepPlayer<LightShadowAngleTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override float GetValue(Light component) => component.shadowAngle;
        /// <inheritdoc />
        protected override void SetValue(Light component, float value) => component.shadowAngle = value;
    }
}
