using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening preferred height on a LayoutElement.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(LayoutElementPreferredHeightTweenStepPlayer), "LayoutElement/PreferredHeight", "PreferredHeight", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutElementPreferredHeightTweenStepDefinition : FloatTweenStepDefinition<LayoutElement>
    {
    }
}
