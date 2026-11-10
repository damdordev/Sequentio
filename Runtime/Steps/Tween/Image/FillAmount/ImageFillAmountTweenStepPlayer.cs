using UnityEngine.UI;

namespace Damdor.Sequentio
{
    public class ImageFillAmountTweenStepPlayer : FloatTweenStepPlayer<ImageFillAmountTweenStepDefinition, Image>
    {
        protected override float GetValue(Image component) => component.fillAmount;
        protected override void SetValue(Image component, float value) => component.fillAmount = value;
    }
}
