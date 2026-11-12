using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening an image fill amount.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(ImageFillAmountTweenStepPlayer), "Image/FillAmount", "FillAmount", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ImageFillAmountTweenStepDefinition : FloatTweenStepDefinition<Image>
    {
    }
}
