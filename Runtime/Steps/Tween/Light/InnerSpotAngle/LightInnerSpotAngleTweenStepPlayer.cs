using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the LightInnerSpotAngleTween step.
    /// </summary>
    public class LightInnerSpotAngleTweenStepPlayer : FloatTweenStepPlayer<LightInnerSpotAngleTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override float GetValue(Light component) => component.innerSpotAngle;
        /// <inheritdoc />
        protected override void SetValue(Light component, float value) => component.innerSpotAngle = value;
    }
}
