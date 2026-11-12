using System;
using TMPro;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Defines a step for tweening character width adjustment on a TextMesh Pro component.
    /// </summary>
    [Serializable]
    [SequentioStep(typeof(TMPTextCharacterWidthAdjustmentTweenStepPlayer), "TMP_Text/CharacterWidthAdjustment", "CharacterWidthAdjustment", "Packages/com.damdor.sequentio/Editor/StepsIcons/position.png")]
    public class TMPTextCharacterWidthAdjustmentTweenStepDefinition : FloatTweenStepDefinition<TMP_Text>
    {
    }
}
