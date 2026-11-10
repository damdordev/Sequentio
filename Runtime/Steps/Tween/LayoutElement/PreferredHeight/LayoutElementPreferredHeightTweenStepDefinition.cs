using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(LayoutElementPreferredHeightTweenStepPlayer), "LayoutElement/PreferredHeight", "PreferredHeight", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutElementPreferredHeightTweenStepDefinition : FloatTweenStepDefinition<LayoutElement>
    {
    }
}
