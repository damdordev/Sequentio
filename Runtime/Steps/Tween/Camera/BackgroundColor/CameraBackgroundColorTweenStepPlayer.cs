using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Camera.backgroundColor.
    /// </summary>
    public class CameraBackgroundColorTweenStepPlayer : ColorTweenStepPlayer<CameraBackgroundColorTweenStepDefinition, Camera>
    {
        /// <inheritdoc />
        protected override Color GetValue(Camera component) => component.backgroundColor;
        /// <inheritdoc />
        protected override void SetValue(Camera component, Color value) => component.backgroundColor = value;
    }
}
