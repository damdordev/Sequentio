using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting whether an image fills its center.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetImageFillCenterStepPlayer), "Image/FillCenter", "Fill Center", "Packages/com.damdor.sequentio/Editor/StepsIcons/SetImageFillCenterStep_icon.png")]
    public class SetImageFillCenterStepDefinition : ValueImmediateStepDefinition<Image, bool>
    {
    }
}
