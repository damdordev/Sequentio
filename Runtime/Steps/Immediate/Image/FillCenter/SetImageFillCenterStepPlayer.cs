using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class SetImageFillCenterStepPlayer : ValueImmediateStepPlayer<SetImageFillCenterStepDefinition, Image, bool>
    {
        protected override bool GetValue(Image target) => target.fillCenter;
        protected override void SetValue(Image target, bool value) => target.fillCenter = value;
    }
}
