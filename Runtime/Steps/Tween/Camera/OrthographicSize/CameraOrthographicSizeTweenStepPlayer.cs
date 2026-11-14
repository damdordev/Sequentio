using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening Camera.orthographicSize.
    /// </summary>
    public class CameraOrthographicSizeTweenStepPlayer : FloatTweenStepPlayer<CameraOrthographicSizeTweenStepDefinition, Camera>
    {
        /// <inheritdoc />
        protected override float GetValue(Camera component) => component.orthographicSize;
        /// <inheritdoc />
        protected override void SetValue(Camera component, float value) => component.orthographicSize = value;
    }
}
