using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(LayoutElementMinWidthTweenStepPlayer), "LayoutElement/MinWidth", "MinWidth", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutElementMinWidthTweenStepDefinition : FloatTweenStepDefinition<LayoutElement>
    {
    }
}
