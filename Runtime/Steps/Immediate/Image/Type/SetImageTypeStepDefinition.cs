using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for setting the image rendering type.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(SetImageTypeStepPlayer), "Image/Type", "Type", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class SetImageTypeStepDefinition : ValueImmediateStepDefinition<Image, Image.Type>
    {
    }
}
