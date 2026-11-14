using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Light.range.
    /// </summary>
    public class LightRangeTweenStepPlayer : FloatTweenStepPlayer<LightRangeTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override float GetValue(Light component) => component.range;
        /// <inheritdoc />
        protected override void SetValue(Light component, float value) => component.range = value;
    }
}
