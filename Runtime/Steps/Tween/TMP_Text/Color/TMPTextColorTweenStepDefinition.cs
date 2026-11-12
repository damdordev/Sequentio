using System;
using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening color on a TextMesh Pro component.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(TMPTextColorTweenStepPlayer), "TMP_Text/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextColorTweenStepDefinition : ColorTweenStepDefinition<TMP_Text>
    {
    }
}
