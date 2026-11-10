using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(ImageFillAmountTweenStepPlayer), "Image/FillAmount", "FillAmount", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class ImageFillAmountTweenStepDefinition : FloatTweenStepDefinition<Image>
    {
    }
}
