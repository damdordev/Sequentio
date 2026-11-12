using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening min width on a LayoutElement.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LayoutElementMinWidthTweenStepPlayer), "LayoutElement/MinWidth", "MinWidth", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutElementMinWidthTweenStepDefinition : FloatTweenStepDefinition<LayoutElement>
    {
    }
}
