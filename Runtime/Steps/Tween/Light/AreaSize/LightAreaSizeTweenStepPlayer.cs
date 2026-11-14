using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Light.areaSize.
    /// </summary>
    public class LightAreaSizeTweenStepPlayer : Vector2TweenStepPlayer<LightAreaSizeTweenStepDefinition, Light>
    {
        /// <inheritdoc />
        protected override Vector2 GetValue(Light component) => component.areaSize;
        /// <inheritdoc />
        protected override void SetValue(Light component, Vector2 value) => component.areaSize = value;
    }
}
