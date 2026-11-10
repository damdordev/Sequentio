using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(SetImageTypeStepPlayer), "Image/Type", "Type", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetImageTypeStepDefinition : ValueImmediateStepDefinition<Image, Image.Type>
    {
    }
}
