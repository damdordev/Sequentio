using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening preferred width on a LayoutElement.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LayoutElementPreferredWidthTweenStepPlayer), "LayoutElement/PreferredWidth", "PreferredWidth", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutElementPreferredWidthTweenStepDefinition : FloatTweenStepDefinition<LayoutElement>
    {
    }
}
