using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the RawImageUVRectTween step.
    /// </summary>
    public class RawImageUVRectTweenStepPlayer : TweenStepPlayer<RawImageUVRectTweenStepDefinition, RawImage, Rect>
    {
        /// <inheritdoc />
        protected override Rect GetValue(RawImage component) => component.uvRect;
        /// <inheritdoc />
        protected override void SetValue(RawImage component, Rect value) => component.uvRect = value;
    }
}
