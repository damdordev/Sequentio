using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Camera.fieldOfView.
    /// </summary>
    public class CameraFieldOfViewTweenStepPlayer : FloatTweenStepPlayer<CameraFieldOfViewTweenStepDefinition, Camera>
    {
        /// <inheritdoc />
        protected override float GetValue(Camera component) => component.fieldOfView;
        /// <inheritdoc />
        protected override void SetValue(Camera component, float value) => component.fieldOfView = value;
    }
}
