using System;
using TMPro;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(TMPTextColorTweenStepPlayer), "TMP_Text/Color", "Color", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextColorTweenStepDefinition : ColorTweenStepDefinition<TMP_Text>
    {
    }
}
