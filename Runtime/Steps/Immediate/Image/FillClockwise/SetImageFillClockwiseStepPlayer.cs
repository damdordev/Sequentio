using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting whether an image fills clockwise.
    /// </summary>
    public class SetImageFillClockwiseStepPlayer : ValueImmediateStepPlayer<SetImageFillClockwiseStepDefinition, Image, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(Image target) => target.fillClockwise;
        /// <inheritdoc />
        protected override void SetValue(Image target, bool value) => target.fillClockwise = value;
    }
}
