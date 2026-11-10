using System;
using UnityEngine.UI;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(LayoutElementMinHeightTweenStepPlayer), "LayoutElement/MinHeight", "MinHeight", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class LayoutElementMinHeightTweenStepDefinition : FloatTweenStepDefinition<LayoutElement>
    {
    }
}
