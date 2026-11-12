using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for setting the image rendering type.
    /// </summary>
    public class SetImageTypeStepPlayer : ValueImmediateStepPlayer<SetImageTypeStepDefinition, Image, Image.Type>
    {
        /// <inheritdoc />
        protected override Image.Type GetValue(Image target) => target.type;
        /// <inheritdoc />
        protected override void SetValue(Image target, Image.Type value) => target.type = value;
    }
}
