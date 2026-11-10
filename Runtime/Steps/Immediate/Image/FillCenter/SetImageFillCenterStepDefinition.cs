using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetImageFillCenterStepPlayer), "Image/FillCenter", "FillCenter", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetImageFillCenterStepDefinition : ValueImmediateStepDefinition<Image, bool>
    {
    }
}
