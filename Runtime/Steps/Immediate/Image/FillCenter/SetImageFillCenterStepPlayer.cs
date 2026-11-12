using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting whether an image fills its center.
    /// </summary>
    public class SetImageFillCenterStepPlayer : ValueImmediateStepPlayer<SetImageFillCenterStepDefinition, Image, bool>
    {
        /// <inheritdoc />
        protected override bool GetValue(Image target) => target.fillCenter;
        /// <inheritdoc />
        protected override void SetValue(Image target, bool value) => target.fillCenter = value;
    }
}
