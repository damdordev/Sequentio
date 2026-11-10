using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class SetImageTypeStepPlayer : ValueImmediateStepPlayer<SetImageTypeStepDefinition, Image, Image.Type>
    {
        protected override Image.Type GetValue(Image target) => target.type;
        protected override void SetValue(Image target, Image.Type value) => target.type = value;
    }
}
