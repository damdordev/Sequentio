using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening min height on a LayoutElement.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LayoutElementMinHeightTweenStepPlayer), "LayoutElement/MinHeight", "MinHeight", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutElementMinHeightTweenStepDefinition : FloatTweenStepDefinition<LayoutElement>
    {
    }
}
