using System;
using UnityEngine;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines the Slider/Value step.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SliderValueTweenStepPlayer), "Slider/Value", "Value", "Packages/com.damdor.sequentio/Editor/StepsIcons/SliderValueTweenStep_icon.png")]
    public class SliderValueTweenStepDefinition : FloatTweenStepDefinition<Slider>
    {
    }
}
