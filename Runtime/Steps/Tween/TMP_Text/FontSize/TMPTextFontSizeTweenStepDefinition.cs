using System;
using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening font size on a TextMesh Pro component.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(TMPTextFontSizeTweenStepPlayer), "TMP_Text/FontSize", "FontSize", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextFontSizeTweenStepDefinition : FloatTweenStepDefinition<TMP_Text>
    {
    }
}
