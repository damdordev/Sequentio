using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting whether an image fills its center.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetImageFillCenterStepPlayer), "Image/FillCenter", "FillCenter", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetImageFillCenterStepDefinition : ValueImmediateStepDefinition<Image, bool>
    {
    }
}
