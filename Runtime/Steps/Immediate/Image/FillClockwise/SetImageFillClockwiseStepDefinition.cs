using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetImageFillClockwiseStepPlayer), "Image/FillClockwise", "FillClockwise", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetImageFillClockwiseStepDefinition : ValueImmediateStepDefinition<Image, bool>
    {
    }
}
