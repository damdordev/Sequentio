using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class SetImageFillClockwiseStepPlayer : ValueImmediateStepPlayer<SetImageFillClockwiseStepDefinition, Image, bool>
    {
        protected override bool GetValue(Image target) => target.fillClockwise;
        protected override void SetValue(Image target, bool value) => target.fillClockwise = value;
    }
}
