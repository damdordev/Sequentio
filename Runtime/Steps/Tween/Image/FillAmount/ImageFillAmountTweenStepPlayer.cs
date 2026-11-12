using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays a step for tweening an image fill amount.
    /// </summary>
    public class ImageFillAmountTweenStepPlayer : FloatTweenStepPlayer<ImageFillAmountTweenStepDefinition, Image>
    {
        /// <inheritdoc />
        protected override float GetValue(Image component) => component.fillAmount;
        /// <inheritdoc />
        protected override void SetValue(Image component, float value) => component.fillAmount = value;
    }
}
