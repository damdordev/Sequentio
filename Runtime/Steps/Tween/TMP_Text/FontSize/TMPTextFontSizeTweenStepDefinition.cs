using System;
using TMPro;

namespace Damdor.Sequentio
{
    [Serializable]
    [SequentioStep(typeof(TMPTextFontSizeTweenStepPlayer), "TMP_Text/FontSize", "FontSize", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextFontSizeTweenStepDefinition : FloatTweenStepDefinition<TMP_Text>
    {
    }
}
