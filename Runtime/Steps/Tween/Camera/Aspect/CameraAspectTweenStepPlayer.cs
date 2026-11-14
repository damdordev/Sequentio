using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Camera.aspect.
    /// </summary>
    public class CameraAspectTweenStepPlayer : FloatTweenStepPlayer<CameraAspectTweenStepDefinition, Camera>
    {
        /// <inheritdoc />
        protected override float GetValue(Camera component) => component.aspect;
        /// <inheritdoc />
        protected override void SetValue(Camera component, float value) => component.aspect = value;
    }
}
