using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Plays the SliderValueTween step.
    /// </summary>
    public class SliderValueTweenStepPlayer : FloatTweenStepPlayer<SliderValueTweenStepDefinition, Slider>
    {
        /// <inheritdoc />
        protected override float GetValue(Slider component) => component.value;
        /// <inheritdoc />
        protected override void SetValue(Slider component, float value) => component.value = value;
    }
}
