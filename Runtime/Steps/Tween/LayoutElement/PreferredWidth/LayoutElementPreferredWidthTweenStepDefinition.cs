using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(LayoutElementPreferredWidthTweenStepPlayer), "LayoutElement/PreferredWidth", "PreferredWidth", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutElementPreferredWidthTweenStepDefinition : FloatTweenStepDefinition<LayoutElement>
    {
    }
}
